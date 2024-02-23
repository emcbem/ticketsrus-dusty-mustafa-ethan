create role azure_pg_admin;
create role shared;
--
-- PostgreSQL database dump
--

-- Dumped from database version 16.0
-- Dumped by pg_dump version 16.1 (Debian 16.1-1.pgdg120+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: ticketsdb; Type: SCHEMA; Schema: -; Owner: azure_pg_admin
--

CREATE SCHEMA ticketsdb;


ALTER SCHEMA ticketsdb OWNER TO postgres;
GRANT ALL ON SCHEMA ticketsdb TO shared;
GRANT ALL ON SCHEMA ticketsdb TO azure_pg_admin;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: event; Type: TABLE; Schema: ticketsdb; Owner: shared
--

CREATE TABLE ticketsdb.event (
    id integer NOT NULL,
    name character varying(100) NOT NULL,
    lastupdated timestamp with time zone DEFAULT CURRENT_DATE NOT NULL,
    eventdate timestamp with time zone NOT NULL
);


ALTER TABLE ticketsdb.event OWNER TO shared;

--
-- Name: event_id_seq; Type: SEQUENCE; Schema: ticketsdb; Owner: shared
--

ALTER TABLE ticketsdb.event ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME ticketsdb.event_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: ticket; Type: TABLE; Schema: ticketsdb; Owner: shared
--

CREATE TABLE ticketsdb.ticket (
    id uuid NOT NULL,
    eventid integer NOT NULL,
    name character varying(100) NOT NULL,
    isscanned boolean NOT NULL,
    lastupdated timestamp with time zone DEFAULT CURRENT_DATE NOT NULL
);


ALTER TABLE ticketsdb.ticket OWNER TO shared;

--
-- Data for Name: event; Type: TABLE DATA; Schema: ticketsdb; Owner: shared
--

COPY ticketsdb.event (id, name, lastupdated, eventdate) FROM stdin;
1	Lady Gaga	2024-02-13 00:00:00+00	2024-02-13 01:37:25.582613+00
2	Usher	2024-02-13 00:00:00+00	2024-02-13 01:47:14.71545+00
\.


--
-- Data for Name: ticket; Type: TABLE DATA; Schema: ticketsdb; Owner: shared
--

COPY ticketsdb.ticket (id, eventid, name, isscanned, lastupdated) FROM stdin;
\.


--
-- Name: event_id_seq; Type: SEQUENCE SET; Schema: ticketsdb; Owner: shared
--

SELECT pg_catalog.setval('ticketsdb.event_id_seq', 2, true);


--
-- Name: event event_pkey; Type: CONSTRAINT; Schema: ticketsdb; Owner: shared
--

ALTER TABLE ONLY ticketsdb.event
    ADD CONSTRAINT event_pkey PRIMARY KEY (id);


--
-- Name: ticket ticket_pkey; Type: CONSTRAINT; Schema: ticketsdb; Owner: shared
--

ALTER TABLE ONLY ticketsdb.ticket
    ADD CONSTRAINT ticket_pkey PRIMARY KEY (id);


--
-- Name: ticket ticket_eventid_fkey; Type: FK CONSTRAINT; Schema: ticketsdb; Owner: shared
--

ALTER TABLE ONLY ticketsdb.ticket
    ADD CONSTRAINT ticket_eventid_fkey FOREIGN KEY (eventid) REFERENCES ticketsdb.event(id);


--
-- PostgreSQL database dump complete
--

