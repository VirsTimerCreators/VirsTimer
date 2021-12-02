## Virstimer.Server

### 

*To run application locally, a mongo application should be available on port 27017*

To run program use gradle wrapper (gradlew or gradlew.bat depending on your OS)

```./gradlew.bat bootRun``` on windows, or
```./gradlew bootRun``` should start the application


To verify that application is running successfully 

``` curl -I localhost:8080/actuator/info```

should return something like this:
```
HTTP/1.1 200
Content-Type: application/vnd.spring-boot.actuator.v3+json
Content-Length: 2
Date: Sat, 29 May 2021 14:56:54 GMT
```


### Authentication
For application to run and provide authorization role definitions in db are required. To set them up connect to MongoDB (for example using Robo3T gui)

Then run following script in mongo shell:
```
db.roles.insertMany([
   { name: "ROLE_USER" },
   { name: "ROLE_MODERATOR" },
   { name: "ROLE_ADMIN" },
])
```

### Documentation

To access configuration, run application and visit:
```
http://localhost:8080/swagger-ui.html
```

### TODO:
sudo docker run -p 8080:8080 virstimer-test-2 -it --network="host"