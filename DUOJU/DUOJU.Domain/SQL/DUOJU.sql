use master

if db_id('DUOJU') is not null drop database DUOJU
go

create database DUOJU
on primary (
	name = DUOJU_mdf,
	filename='d:\data\DUOJU.mdf'
)
log on (
	name = DUOJU_ldf,
	filename = 'd:\data\DUOJU.ldf'
)
go

use DUOJU

create table DUOJU$COUNTRIES
(
	COUNTRY_ID int primary key identity (1,1) not null,
	COUNTRY_CODE varchar(5) not null,
	COUNTRY_NAME varchar(50) not null,
	COUNTRY_NAME_CN nvarchar(25) not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_TIME datetime default getdate() not null
)
go
create unique index UX_DUOJU$COUNTRY_CODE on DUOJU$COUNTRIES(COUNTRY_CODE)
create index IX_DUOJU$COUNTRY_NAME on DUOJU$COUNTRIES(COUNTRY_NAME)
create index IX_DUOJU$COUNTRY_NAMECN on DUOJU$COUNTRIES(COUNTRY_NAME_CN)
go

create table DUOJU$PROVINCES
(
	PROVINCE_ID int primary key identity (1,1) not null,
	COUNTRY_ID int references DUOJU$COUNTRIES(COUNTRY_ID) not null,
	PROVINCE_NAME varchar(50) not null,
	PROVINCE_NAME_CN nvarchar(25) not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_TIME datetime default getdate() not null
)
go
create index IX_DUOJU$PROVINCE_CTID on DUOJU$PROVINCES(COUNTRY_ID)
create index IX_DUOJU$PROVINCE_NAME on DUOJU$PROVINCES(PROVINCE_NAME)
create index IX_DUOJU$PROVINCE_NAMECN on DUOJU$PROVINCES(PROVINCE_NAME_CN)
go

create table DUOJU$CITIES
(
	CITY_ID int primary key identity (1,1) not null,
	COUNTRY_ID int references DUOJU$COUNTRIES(COUNTRY_ID) null,
	PROVINCE_ID int references DUOJU$PROVINCES(PROVINCE_ID) null,
	CITY_NAME varchar(50) not null,
	CITY_NAME_CN nvarchar(25) not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_TIME datetime default getdate() not null
)
go
create index IX_DUOJU$CITY_CTID on DUOJU$CITIES(COUNTRY_ID)
create index IX_DUOJU$CITY_PVID on DUOJU$CITIES(PROVINCE_ID)
create index IX_DUOJU$CITY_NAME on DUOJU$CITIES(CITY_NAME)
create index IX_DUOJU$CITY_NAMECN on DUOJU$CITIES(CITY_NAME_CN)
alter table DUOJU$CITIES add constraint CK_DUOJU$CITY_CTID_PVID check (not (COUNTRY_ID is null and PROVINCE_ID is null))
go

create table DUOJU$ROLE_PRIVILEGES
(
	ROLE varchar(20) primary key not null,
	PRIVILEGES varchar(500) not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_TIME datetime default getdate() not null
)
go
insert into DUOJU$ROLE_PRIVILEGES (ROLE, PRIVILEGES) values ('MANAGER', '')
insert into DUOJU$ROLE_PRIVILEGES (ROLE, PRIVILEGES) values ('SUPPLIER', '')
insert into DUOJU$ROLE_PRIVILEGES (ROLE, PRIVILEGES) values ('USER', '')
go

create table DUOJU$IMAGE_CATEGORIES
(
	IMAGE_CATEGORY_ID int primary key identity (1,1) not null,
	DESCRIPTION nvarchar(200) null,
	CREATE_BY int default 0 not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_BY int default 0 not null,
	LAST_UPDATE_TIME datetime default getdate() not null
)
go

create table DUOJU$IMAGES
(
	IMAGE_ID int primary key identity (1,1) not null,
	IMAGE_CATEGORY_ID int references DUOJU$IMAGE_CATEGORIES(IMAGE_CATEGORY_ID) not null,
	DESCRIPTION nvarchar(200) null,
	URL varchar(300) null,
	SORTORDER int default 0 null,
	CREATE_BY int default 0 not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_BY int default 0 not null,
	LAST_UPDATE_TIME datetime default getdate() not null
)
go

create table DUOJU$SUPPLIERS
(
	SUPPLIER_ID int primary key identity (1,1) not null,
	NAME nvarchar(50) null,
	ADDRESS nvarchar(200) null,
	TELPHONE varchar(20) null,
	FAX varchar(20) null,
	EMAIL varchar(50) null,
	QQ varchar(20) null,
	WEBSITE varchar(200) null,
	PROVINCE_ID int references DUOJU$PROVINCES(PROVINCE_ID) null,
	CITY_ID int references DUOJU$CITIES(CITY_ID) null,
	LONGITUDE float null,
	LATITUDE float null,
	CREATE_BY int default 0 not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_BY int default 0 not null,
	LAST_UPDATE_TIME datetime default getdate() not null
)
go

create table DUOJU$SUPPLIER_INFOS
(
	SUPPLIER_INFO_ID int primary key identity (1,1) not null,
	SUPPLIER_ID int references DUOJU$SUPPLIERS(SUPPLIER_ID) not null,
	MAIN_TITLE nvarchar(100) null,
	SUBTITLE nvarchar(100) null,
	VIEW_COUNT int default 0 null,
	PARTY_COUNT int default 0 null,
	FAVOUR_COUNT int default 0 null,
	CONTENT text null,
	CREATE_BY int default 0 not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_BY int default 0 not null,
	LAST_UPDATE_TIME datetime default getdate() not null
)
go

create table DUOJU$SUPPLIER_IMAGES
(
	SUPPLIER_IMAGE_ID int primary key identity (1,1) not null,
	SUPPLIER_ID int references DUOJU$SUPPLIERS(SUPPLIER_ID) not null,
	IMAGE_ID int references DUOJU$IMAGES(IMAGE_ID) not null,
	CREATE_BY int default 0 not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_BY int default 0 not null,
	LAST_UPDATE_TIME datetime default getdate() not null
)
go

create table DUOJU$USERS
(
	USER_ID int primary key identity (1,1) not null,
	ACCOUNT varchar(70) not null,
	PASSWORD varchar(70) null,
	SOURCE int not null,
	ROLE varchar(20) references DUOJU$ROLE_PRIVILEGES(ROLE) not null,
	PRIVILEGES varchar(500) not null,
	SUBSCRIBED char(1) null,
	SUBSCRIBE_TIME datetime null,
	OPEN_ID varchar(70) null,
	NICK_NAME nvarchar(70) null,
	SEX char(1) null,
	HEAD_IMG_URL varchar(300) null,
	COUNTRY_ID int references DUOJU$COUNTRIES(COUNTRY_ID) null,
	PROVINCE_ID int references DUOJU$PROVINCES(PROVINCE_ID) null,
	CITY_ID int references DUOJU$CITIES(CITY_ID) null,
	ENABLED char(1) not null,
	CREATE_BY int default 0 not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_BY int default 0 not null,
	LAST_UPDATE_TIME datetime default getdate() not null
)
go
create unique index UX_DUOJU$USER_ACCOUNT on DUOJU$USERS(ACCOUNT)
create unique index UX_DUOJU$USER_OPENID on DUOJU$USERS(OPEN_ID)
create index IX_DUOJU$USER_ROLE on DUOJU$USERS(ROLE)
alter table DUOJU$USERS add constraint CK_DUOJU$USER_SOURCE check (SOURCE in (0, 1))
alter table DUOJU$USERS add constraint CK_DUOJU$USER_SUBSCRIBED check (SUBSCRIBED in ('Y', 'N'))
alter table DUOJU$USERS add constraint CK_DUOJU$USER_SEX check (SEX in ('U', 'M', 'F'))
alter table DUOJU$USERS add constraint CK_DUOJU$USER_ENABLED check (ENABLED in ('Y', 'N'))
go

create table DUOJU$USER_FINANCES
(
	USER_FINANCE_ID int primary key identity (1,1) not null,
	USER_ID int references DUOJU$USERS(USER_ID) not null,
	COIN_COUNT float not null,
	CREATE_BY int default 0 not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_BY int default 0 not null,
	LAST_UPDATE_TIME datetime default getdate() not null
)
go
create index IX_DUOJU$USER_FINANCE_USERID on DUOJU$USER_FINANCES(USER_ID)
go

create table DUOJU$IDENTIFIERS
(
	IDENTIFIER_ID int primary key identity (1,1) not null,
	IDENTIFIER_TYPE int not null,
	IDENTIFIER_NO varchar(20) not null,
	EXPIRES_TIME datetime not null,
	STATUS int not null,
	CREATE_BY int default 0 not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_BY int default 0 not null,
	LAST_UPDATE_TIME datetime default getdate() not null
)
go
create unique index UX_DUOJU$IDENTIFIER_NO on DUOJU$IDENTIFIERS(IDENTIFIER_NO)
alter table DUOJU$IDENTIFIERS add constraint CK_DUOJU$IDENTIFIER_TYPE check (IDENTIFIER_TYPE in (0, 1))
alter table DUOJU$IDENTIFIERS add constraint CK_DUOJU$IDENTIFIER_STATUS check (STATUS in (0, 6))
go

create table DUOJU$IDENTIFIER_SETTINGS
(
	IDENTIFIER_SETTING_ID int primary key identity (1,1) not null,
	IDENTIFIER_ID int references DUOJU$IDENTIFIERS(IDENTIFIER_ID) not null,
	SETTING_CODE varchar(20) not null,
	SETTING_VALUE varchar(20) not null,
	CREATE_BY int default 0 not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_BY int default 0 not null,
	LAST_UPDATE_TIME datetime default getdate() not null
)
go
create index IX_DUOJU$IDENTIFIER_SETTING_IDENTIID on DUOJU$IDENTIFIER_SETTINGS(IDENTIFIER_ID)
go

create table DUOJU$PARTIES
(
	PARTY_ID int primary key identity (1,1) not null,
	SUPPLIER_ID int references DUOJU$SUPPLIERS(SUPPLIER_ID) not null,
	INITIATOR_ID int references DUOJU$USERS(USER_ID) not null,
	HOLD_DATE datetime not null,
	HOLD_TIME int not null,
	DESCRIPTION nvarchar(100) null,
	MIN_INTO_FORCE int not null,
	MAX_INTO_FORCE int null,
	CONSUMPTION_VOUCHER_ID int references DUOJU$IDENTIFIERS(IDENTIFIER_ID) null,
	STATUS int not null,
	CREATE_BY int default 0 not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_BY int default 0 not null,
	LAST_UPDATE_TIME datetime default getdate() not null
)
go
create index IX_DUOJU$PARTY_SUPPID on DUOJU$PARTIES(SUPPLIER_ID)
create index IX_DUOJU$PARTY_INITID on DUOJU$PARTIES(INITIATOR_ID)
alter table DUOJU$PARTIES add constraint CK_DUOJU$PARTY_HOLDTIME check (HOLD_TIME in (0, 1, 2, 3, 4, 5))
alter table DUOJU$PARTIES add constraint CK_DUOJU$PARTY_MININTOFORCE check (MIN_INTO_FORCE >= 2)
alter table DUOJU$PARTIES add constraint CK_DUOJU$PARTY_MAXINTOFORCE check (MAX_INTO_FORCE >= MIN_INTO_FORCE)
alter table DUOJU$PARTIES add constraint CK_DUOJU$PARTY_STATUS check (STATUS in (-1, 1, 2, 3, 4))
go

create table DUOJU$PARTY_PARTICIPANTS
(
	PARTY_PARTICIPANT_ID int primary key identity (1,1) not null,
	PARTY_ID int references DUOJU$PARTIES(PARTY_ID) not null,
	PARTICIPANT_ID int references DUOJU$USERS(USER_ID) not null,
	PARTICIPATE_TIME datetime not null,
	STATUS int not null,
	CREATE_BY int default 0 not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_BY int default 0 not null,
	LAST_UPDATE_TIME datetime default getdate() not null
);
go
create index IX_DUOJU$PARTY_PARTICIPANT_PARTYID on DUOJU$PARTY_PARTICIPANTS(PARTY_ID)
create index IX_DUOJU$PARTY_PARTICIPANT_PTCPTID on DUOJU$PARTY_PARTICIPANTS(PARTICIPANT_ID)
create index IX_DUOJU$PARTY_PARTICIPANT_STATUS on DUOJU$PARTY_PARTICIPANTS(STATUS)
alter table DUOJU$PARTY_PARTICIPANTS add constraint CK_DUOJU$PARTY_PARTICIPANT_STATUS check (STATUS in (-2, -1, 5))
go

create table DUOJU$PARTY_COMMENTS
(
	PARTY_COMMENT_ID int primary key identity (1,1) not null,
	SUPPLIER_ID int references DUOJU$SUPPLIERS(SUPPLIER_ID) not null,
	PARTY_ID int references DUOJU$PARTIES(PARTY_ID) not null,
	USER_ID int references DUOJU$USERS(USER_ID) not null,
	CONTENT nvarchar(100) not null,
	STATUS int not null,
	CREATE_BY int default 0 not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_BY int default 0 not null,
	LAST_UPDATE_TIME datetime default getdate() not null
);
go
create index IX_DUOJU$PARTY_COMMENT_SUPPID on DUOJU$PARTY_COMMENTS(SUPPLIER_ID)
create index IX_DUOJU$PARTY_COMMENT_PARTYID on DUOJU$PARTY_COMMENTS(PARTY_ID)
create index IX_DUOJU$PARTY_COMMENT_PTCPTID on DUOJU$PARTY_COMMENTS(USER_ID)
create index IX_DUOJU$PARTY_COMMENT_STATUS on DUOJU$PARTY_COMMENTS(STATUS)
alter table DUOJU$PARTY_COMMENTS add constraint CK_DUOJU$PARTY_COMMENT_STATUS check (STATUS in (-1, 0, 7))
go
