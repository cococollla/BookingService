--liquibase formatted sql

--changeset n.beliaev:create-test-table
--comment: Create test table

CREATE TABLE public.Test (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);