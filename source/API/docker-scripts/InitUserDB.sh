#!/bin/bash
set -e
psql -v ON_ERROR_STOP=1 \
--username "$POSTGRES_USER" \
--dbname "$POSTGRES_DB" \
-v app_db="$POSTGRES_APP_DB" \
-v app_user="$POSTGRES_USER" \
<<-EOSQL
  CREATE DATABASE :app_db;
  GRANT ALL PRIVILEGES ON DATABASE :app_db to :app_user;
EOSQL

psql -v ON_ERROR_STOP=1 \
--username "$POSTGRES_USER" \
--dbname "$POSTGRES_APP_DB" \
-v app_user="$POSTGRES_USER" \
<<-EOSQL
  GRANT ALL ON SCHEMA public to :app_user;
  
  CREATE TABLE products(
    id VARCHAR(50) PRIMARY KEY,
    name TEXT NOT NULL,
    brand TEXT NOT NULL,
    price NUMERIC(8,2) NOT NULL,
    UNIQUE(name, brand)
  );
EOSQL