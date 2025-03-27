MSpec is referred to as a "context/specification" test framework due to its structured approach to describing and implementing tests, commonly known as "specs." This approach follows a natural-language-inspired grammar that enhances readability and clarity. The general structure can be expressed as:

"Given that the system is in a specific state, when a particular action occurs, then it should produce a defined outcome or reach an expected end state."

This structure closely aligns with the traditional Arrange-Act-Assert model of unit testing. However, to improve readability and reduce syntactic noise, MSpec departs from the conventional attribute-on-method paradigm. Instead, it leverages custom delegates assigned to anonymous methods, encouraging a consistent naming convention to enhance test clarity and maintainability.
