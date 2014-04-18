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
	COUNTRY_CODE int not null,
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

create table DUOJU$USERS
(
	USER_ID int primary key identity (1,1) not null,
	ACCOUNT varchar(50) not null,
	PASSWORD varchar(50) null,
	FROM_TYPE int check (FROM_TYPE in (0, 1)) not null,
	ROLE varchar(20) references DUOJU$ROLE_PRIVILEGES(ROLE) not null,
	PRIVILEGES varchar(500) not null,
	SUBSCRIBED char(1) check (SUBSCRIBED in ('Y', 'N')) null,
	SUBSCRIBE_TIME datetime null,
	OPEN_ID varchar(50) null,
	NICK_NAME nvarchar(50) not null,
	SEX char(1) check (SEX in ('M', 'F')) null,
	HEAD_IMG_URL varchar(100) null,
	COUNTRY_ID int references DUOJU$COUNTRIES(COUNTRY_ID) null,
	PROVINCE_ID int references DUOJU$PROVINCES(PROVINCE_ID) null,
	CITY_ID int references DUOJU$CITIES(CITY_ID) null,
	STATUS int check (STATUS in (-1, 0, 1)) not null,
	CREATE_BY int default 0 not null,
	CREATE_TIME datetime default getdate() not null,
	LAST_UPDATE_BY int default 0 not null,
	LAST_UPDATE_TIME datetime default getdate() not null
)
go
create unique index UX_DUOJU$USER_ACCOUNT on DUOJU$USERS(ACCOUNT)
create index IX_DUOJU$USER_ROLE on DUOJU$USERS(ROLE)

create table DUOJU$USER_CREDITS
(
	USER_CREDIT_ID int primary key identity (1,1) not null,
	USER_ID int references DUOJU$USERS(USER_ID) not null,
	CREDIT_AMOUNT float not null,
	CREATE_BY int default 0 not null,
	CREATE_TIME datetime default getdate() not null
)
go
create index IX_DUOJU$USER_CREDIT_USERID on DUOJU$USER_CREDITS(USER_ID)




/*商家基础信息表*/
create table Supplier
(
	SupplierId int identity(1,1),
	Name nvarchar(255) null,
	Address nvarchar(1000) null,
	Telphone nvarchar(255) null,
	Fax nvarchar(255) null,
	Email nvarchar(50) null,
	QQ nvarchar(50) null,
	WebSite nvarchar(100) null,
	Longitude decimal null,
	Latitude decimal null,
	CreatedDate datetime default getdate()
);

/*商家信息表（文案，标题，浏览量等信息）*/
create table SupplierInfo
(
	SupplierInfoId int identity(1,1),
	SupplierId int not null,
	MainTitle nvarchar(500) null,
	SubTitle nvarchar(500) null,
	ViewCount int default 0,
	PartyCount int default 0,
	FavourCount int default 0,
	Content nvarchar(max) null,
	CreatedDate datetime default getdate(),
	LastUpdateTime datetime null
);

/*图片表*/
create table Images
(
	ImageId int identity(1,1),
	SupplierId int not null,
	ImageCategoryId int not null,
	Description nvarchar(200) null,
	Url nvarchar(500) null,
	CreatedDate datetime default getdate(),
	SortOrder int default 0
);

/*图片分类表*/
create table ImageCategory
(
	ImageCategoryId int identity(1,1),
	Description nvarchar(200) null,
	CreatedDate datetime default getdate()
);

/*商家图片表*/
create table SupplierImages
(
	SupplierImageId int identity(1,1),
	SupplierId int not null,
	ImageId int not null,
	CreatedDate datetime default getdate()
);

/*聚会表*/
create table Party
(
	PartyId int identity(1,1),
	SupplierId int not null,
	InitiatorId int not null,
	PartyName nvarchar(500) not null,
	StartDate datetime null,
	EndDate datetime null,
	InviterCount int default 1,
	Note nvarchar(500) null
);

/*聚会参与者表*/
create table Participator
(
	ParticipatorId int identity(1,1),
	PartyId int not null,
	UserId int not null,
	JoinDate datetime default getdate(),
	JoinStatus int default 1,
	Note nvarchar(500) null	
);

/*评论表*/
create table Comment
(
	CommentId int identity(1,1),
	CommentContent nvarchar(500) null,
	UserId int not null,
	PartyId int not null,
	CreatedDate datetime default getdate()
);