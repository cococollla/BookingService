--liquibase formatted sql

--changeset n.beliaev:drop-test-table
--comment: Drop test table

DROP TABLE Test;