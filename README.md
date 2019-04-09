# Weather Web API
A .NET Web API that takes ZIP-code as a parameter, then returns a string including city name, current temperature, time zone in the following format "At the location *$CITY_NAME*, the temperature is *$TEMPERATURE*, and the timezone is *$TIMEZONE*".

## API endpoint
Replace placeholder `{}` with actual value

### Get weather by city ZIP-code
`http://localhost:{}/api/weathertime?zip={}

## Example of Web API using
`http://localhost:{}/api/weathertime?zip=10001 for New-York  
`http://localhost:{}/api/weathertime?zip=02108 for Boston  
`http://localhost:{}/api/weathertime?zip=98101 for Seattle  