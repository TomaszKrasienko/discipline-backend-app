#!/bin/bash
TAG=''
VERSION_TAG=123
REPOSITORY=tomaszkras/discipline-backend-app

TAG='latest'
VERSION_TAG=$TRAVIS_BUILD_NUMBER

docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD
docker build -f ./../discipline-backed-app/Dockerfile -t $REPOSITORY:$TAG -t $REPOSITORY:$VERSION_TAG .
docker push $REPOSITORY:$VERSION_TAG
docker push $REPOSITORY:$TAG