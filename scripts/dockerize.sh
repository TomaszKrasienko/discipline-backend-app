#!/bin/bash
REPOSITORY=tomaszkras/discipline-backend-app

docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD
docker build -t $REPOSITORY