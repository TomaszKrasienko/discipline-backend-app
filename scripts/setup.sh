#!/bin/bash

DB_HOST="localhost"
DB_PORT="6013"
DB_USER="root"
DB_PASSWORD="rootpwd"

docker compose -f docker-compose-environment.yaml up -d

echo 'Compose finished'

while ! pg_isready -h "$DB_HOST" -p "$DB_PORT"; do
    echo "Waiting for PostgreSql"
    sleep 1
done
echo "PostgreSql is ready to use. Creating database for hangfire server"
PGPASSWORD="$DB_PASSWORD" psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" <<EOF
    CREATE DATABASE "discipline-hangfire";
EOF

docker compose -f docker-compose-services.yaml up --build -d
sleep 4
echo "Checking discipline.centre"

STATUS_CODE=$(curl -o /dev/null -s -w "%{http_code}" http://localhost:7001)

if [[ "$STATUS_CODE" -eq 200 ]]; then
    echo "Serwis 'discipline.centre' działa! Kod: $STATUS_CODE"
else
    echo "Serwis nie działa! Kod: $STATUS_CODE. Wycofywanie zmian"
    docker compose -f docker-compose-services.yaml down

    PGPASSWORD="$DB_PASSWORD" psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" <<EOF
DROP DATABASE "discipline-hangfire";
EOF

    docker compose -f docker-compose-environment.yaml down
fi