﻿// Copyright (c) 2017 Colectica. All rights reserved
// See the LICENSE file in the project root for more information.
using Cogs.Common;
using Cogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogs.Model
{
    public class CogsModelBuilder
    {
        public List<CogsError> Errors { get; } = new List<CogsError>();

        private Cogs.Dto.CogsDtoModel dto;
        private CogsModel model;

        public CogsModel Build(Cogs.Dto.CogsDtoModel cogsDtoModel)
        {
            this.dto = cogsDtoModel;
            this.model = new CogsModel();

            // Copy information about articles.
            model.ArticlesPath = dto.ArticlesPath;
            model.ArticleTocEntries.AddRange(dto.ArticleTocEntries);

            // First pass: create object stubs.
            foreach (var id in dto.Identification)
            {
                var property = new Property();
                MapProperty(id, property);
                model.Identification.Add(property);
            }

            string[] itemNames = dto.ItemTypes.Select(x => x.Name).ToArray();

            foreach (var itemTypeDto in dto.ItemTypes)
            {
                var itemType = new ItemType();
                MapDataType(itemTypeDto, itemType, true);
                model.ItemTypes.Add(itemType);
                
                // add identification to all base types in itemtypes
                if (string.IsNullOrEmpty(itemType.ExtendsTypeName))
                {
                    itemType.Properties.InsertRange(0, model.Identification);
                }
                else
                {
                    if (!itemNames.Contains(itemType.ExtendsTypeName))
                    {
                        string errorMessage = $"Item {itemType.Name} can not extend {itemType.ExtendsTypeName} because it is not an item type.";
                        throw new InvalidOperationException(errorMessage);
                    }
                }
            }

            foreach (var reusableTypeDto in dto.ReusableDataTypes)
            {
                var reusableType = new DataType();
                MapDataType(reusableTypeDto, reusableType, false);
                model.ReusableDataTypes.Add(reusableType);
            }

            foreach (var topicIndexDto in dto.TopicIndices)
            {
                var index = new TopicIndex();
                MapTopicIndex(topicIndexDto, index);
                model.TopicIndices.Add(index);
            }
            

            // Second pass: add references between items.
            foreach (var itemType in model.ItemTypes)
            {
                CreateRelationships(itemType);
            }

            foreach (var type in model.ReusableDataTypes)
            {
                CreateRelationships(type);
            }

            foreach (var index in model.TopicIndices)
            {
                foreach (var itemTypeName in index.ItemTypeNames)
                {
                    var includedType = GetTypeByName(itemTypeName);
                    index.ItemTypes.Add(includedType);
                }
            }

            // Third pass: look for relationships among items.
            // Related item types, based on following the properties' data types.
            foreach (var itemType in model.ItemTypes)
            {
                ProcessProperties(itemType.Properties, itemType.Relationships, new HashSet<string>());
            }

            return model;
        }

        private void CreateRelationships(DataType type)
        {
            // Property types
            foreach (var property in type.Properties)
            {
                property.DataType = GetTypeByName(property.DataTypeName);
            }

            // Parents
            string extendsTypeName = type.ExtendsTypeName;
            while (!string.IsNullOrWhiteSpace(extendsTypeName))
            {
                var parent = GetTypeByName(extendsTypeName);
                type.ParentTypes.Insert(0, parent);
                extendsTypeName = parent.ExtendsTypeName;
            }

            // Look through all other types to determine which types extend this one.
            foreach (var otherType in model.ItemTypes.Where(x => x.ExtendsTypeName == type.Name))
            {
                type.ChildTypes.Add(otherType);
            }
            foreach (var otherType in model.ReusableDataTypes.Where(x => x.ExtendsTypeName == type.Name))
            {
                type.ChildTypes.Add(otherType);
            }

        }

        private void ProcessProperties(List<Property> properties, List<Relationship> relationships, HashSet<string> seenTypeNames, string prefixTypeStr = "")
        {
            foreach (var property in properties)
            {
                if (seenTypeNames.Contains(property.DataType?.Name))
                {
                    continue;
                }
                seenTypeNames.Add(property.DataType?.Name);

                // If the type of this property is an ItemType, consider it related.
                if (property.DataType is ItemType it)
                {
                    string nameStr = property.Name;
                    if (!string.IsNullOrWhiteSpace(prefixTypeStr))
                    {
                        nameStr = prefixTypeStr + "/" + nameStr;
                    }
                    var relationship = new Relationship
                    {
                        PropertyName = nameStr,
                        TargetItemType = it
                    };
                    relationships.Add(relationship);
                }

                // If the type is not an item type, dive deeper to see if
                // the regular-type might reference an ItemType.
                else
                {
                    string nameStr = property.Name;
                    if (!string.IsNullOrWhiteSpace(prefixTypeStr))
                    {
                        nameStr = prefixTypeStr + "/" + nameStr;
                    }
                    ProcessProperties(property.DataType.Properties, relationships, seenTypeNames, nameStr);
                }

            }

        }

        private DataType GetTypeByName(string dataTypeName)
        {
            // Try Item Type.
            var itemType = model.ItemTypes.FirstOrDefault(x => x.Name == dataTypeName);
            if (itemType != null)
            {
                return itemType;
            }

            // Try Reusable Type.
            var reusableType = model.ReusableDataTypes.FirstOrDefault(x => x.Name == dataTypeName);
            if (reusableType != null)
            {
                return reusableType;
            }

            // Must be a primitive, or something from outside the system.
            var primitiveType = new DataType();
            primitiveType.Name = dataTypeName;
            primitiveType.IsXmlPrimitive = true;
            return primitiveType;
        }

        private void MapDataType(Cogs.Dto.DataType dto, DataType dataType, bool isItemType)
        {
            dataType.Name = dto.Name;
            dataType.Description = dto.Description;
            dataType.IsAbstract = dto.IsAbstract;
            dataType.IsPrimitive = dto.IsPrimitive;
            dataType.ExtendsTypeName = dto.Extends;
            dataType.DeprecatedNamespace = dto.DeprecatedNamespace;
            dataType.IsDeprecated = dto.IsDeprecated;
            

            foreach (var dtoProperty in dto.Properties)
            {
                var property = new Property();
                MapProperty(dtoProperty, property);
                dataType.Properties.Add(property);
            }

            if (isItemType)
            {
                dataType.Path = $"/item-types/{dataType.Name}/index";
            }
            else
            {
                dataType.Path = $"/reusable-types/{dataType.Name}/index";
            }
        }

        private void MapProperty(Cogs.Dto.Property dto, Property property)
        {
            property.Name = dto.Name;
            property.DataTypeName = dto.DataType;

            property.MinCardinality = dto.MinCardinality;
            if (string.IsNullOrWhiteSpace(property.MinCardinality))
            {
                property.MinCardinality = "0";
            }

            property.MaxCardinality = dto.MaxCardinality;
            property.Description = dto.Description;


            // simple string restrictions
            property.MinLength = dto.MinLength;
            property.MaxLength = dto.MaxLength;
            if (!string.IsNullOrWhiteSpace(dto.Enumeration))
            {                
                string[] parts = dto.Enumeration.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                property.Enumeration = new List<string>(parts);
            }           
            property.Pattern = dto.Pattern;
            // numeric restrictions
            property.MinInclusive = dto.MinInclusive;
            property.MinExclusive = dto.MaxExclusive;
            property.MaxInclusive = dto.MinExclusive;
            property.MaxExclusive = dto.MaxExclusive;

            property.DeprecatedNamespace = dto.DeprecatedNamespace;
            property.DeprecatedElementOrAttribute = dto.DeprecatedElementOrAttribute;
            property.DeprecatedChoiceGroup = dto.DeprecatedChoiceGroup;
        }

        private void MapTopicIndex(Cogs.Dto.TopicIndex dto, TopicIndex topicIndex)
        {
            topicIndex.Name = dto.Name;
            topicIndex.Description = dto.Description;

            topicIndex.ItemTypeNames.AddRange(dto.ItemTypes);
        }

    }
}
