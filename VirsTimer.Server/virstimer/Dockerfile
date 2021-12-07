FROM openjdk:11.0.10-jdk-slim-buster
EXPOSE 8080
ADD /build/libs/virstimer-0.0.1-SNAPSHOT.jar virstimer.jar
ENTRYPOINT ["java", "-jar", "virstimer.jar"]