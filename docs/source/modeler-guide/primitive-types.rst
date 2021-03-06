Primitive Types
-------------------

COGS provides a number of built-in primitive types you can use to define properties on
items in your model.

Simple Types
~~~~~~~~~~~~~~~~~~~~~~

boolean
```````
Represents Boolean values, which are either true or false.    

Facets
    none

string
``````
Represents character strings.    

Facets
    maxLength, minLength, enumeration, pattern                     

boolean      
```````
Represents Boolean values, which are either true or false.    

decimal
```````
Represents arbitrary precision numbers.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive

float
`````
Represents single-precision 32-bit floating-point numbers.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive

double
``````
Represents double-precision 64-bit floating-point numbers.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive         

duration
````````
Represents a duration of time. The pattern for duration is PnYnMnDTnHnMnS,
where nY represents the number of years, nM the number of months, nD the number
of days, T the date/time separator, nH the number of hours, nM the number of
minutes, and nS the number of seconds.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive

dateTime
```````` 
Represents a specific instance of time. The pattern for dateTime is
CCYY-MM-DDThh:mm:ss where CC represents the century, YY the year, MM the month,
and DD the day, preceded by an optional leading negative (-) character to
indicate a negative number. If the negative character is omitted, positive (+)
is assumed. The T is the date/time separator and hh, mm, and ss represent hour,
minute, and second respectively. Additional digits can be used to increase the
precision of fractional seconds if desired. For example, the format ss.ss...
with any number of digits after the decimal point is supported. The fractional
seconds part is optional. This representation may be immediately followed by a
"Z" to indicate Coordinated Universal Time (UTC) or to indicate the time zone.
For example, the difference between the local time and Coordinated Universal
Time, immediately followed by a sign, + or -, followed by the difference from
UTC represented as hh:mm (minutes is required). If the time zone is included,
both hours and minutes must be present.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive         

time
````
Represents an instance of time that recurs every day. The pattern for time is hh:mm:ss.sss with optional time zone indicator.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive         

date
````
Represents a calendar date. The pattern for date is CCYY-MM-DD with optional time zone indicator as allowed for dateTime

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive         

gYearMonth
``````````
Represents a specific Gregorian month in a specific Gregorian year. A set of one-month long, nonperiodic instances. The pattern for gYearMonth is CCYY-MM with optional time zone indicator.

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive         

gYear
`````
Represents a Gregorian year. A set of one-year long, nonperiodic instances. The pattern for gYear is CCYY with optional time zone indicator as allowed for dateTime.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive         

gMonthDay
`````````
Represents a specific Gregorian date that recurs, specifically a day of the year such as the third of May. A gMonthDay is the set of calendar dates. Specifically, it is a set of one-day long, annually periodic instances. The pattern for gMonthDay is --MM-DD with optional time zone indicator as allowed for date

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive         

gDay
````
Represents a Gregorian day that recurs, specifically a day of the month such as the fifth day of the month. A gDay is the space of a set of calendar dates. Specifically, it is a set of one-day long, monthly periodic instances. The pattern for gDay is ---DD with optional time zone indicator as allowed for date

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive         

gMonth
``````
Represents a Gregorian month that recurs every year. A gMonth is the space of a set of calendar months. Specifically, it is a set of one-month long, yearly periodic instances. The pattern for gMonth is --MM-- with optional time zone indicator as allowed for date.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive         

anyURI
``````
Represents a URI as defined by RFC 2396. An anyURI value can be absolute or relative, and may have an optional fragment identifier.    

Facets
    maxLength, minLength


Derived Types
~~~~~~~~~~~~~~~~~~~~~~~

langString
`````````````````````
Represents a character string and an associated language tag (defined by BCP 47).    

Facets
    maxLength, minLength, enumeration, pattern                          

language
````````
Represents natural language identifiers (defined by BCP 47).   

Facets
    None

int
`````````````````````
Represents an integer with a minimum value of -2147483648 and maximum of 2147483647. This data type is derived from long.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive              

nonPositiveInteger
`````````````````````

Represents an integer that is less than or equal to zero. A
nonPositiveIntegerconsists of a negative sign (-) and sequence of decimal
digits. This data type is derived from integer.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive              

negativeInteger
`````````````````````

Represents an integer that is less than zero. Consists of a negative sign (-)
and sequence of decimal digits. This data type is derived from
nonPositiveInteger.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive              

long
`````````````````````
Represents an integer with a minimum value of -9223372036854775808 and maximum of 9223372036854775807. This data type is derived from integer.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive              

int
`````````````````````
Represents an integer with a minimum value of -2147483648 and maximum of 2147483647. This data type is derived from long.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive              

nonNegativeInteger
`````````````````````
Represents an integer that is greater than or equal to zero. This data type is derived from integer.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive              

unsignedLong
`````````````````````
Represents an integer with a minimum of zero and maximum of 18446744073709551615. This data type is derived from nonNegativeInteger.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive

positiveInteger
`````````````````````

Represents an integer that is greater than zero. This data type is derived from
nonNegativeInteger.    

Facets
    minInclusive, minExclusive, maxInclusive, maxExclusive

cogsDate
````````

A union of dateTime, date, gYearMonth, gYear, and duration which allows for the
use of a date-time combination (YYYY-MM-DDTHH:MM:SS), date (YYYYY-MM-DD),
year-month (YYYY-MM), year (YYYY), and duration (PnYnMnDnHnMnS) as valid
values.    

Facets
    None
