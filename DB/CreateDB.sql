Create Table Department(
Id uniqueidentifier primary key,
Name nvarchar(200) not null,
)

Create Table Employee(
Id uniqueidentifier primary key,
Name nvarchar(150) not null,
Card nvarchar(100),
Cabinet nvarchar(100),
CityPhone nvarchar(100),
InternalPhone nvarchar(100),
Post nvarchar(100),
DepartmentId uniqueidentifier foreign key references Department(Id),
)

Create Table Computer(
Id uniqueidentifier primary key,
Segment nvarchar(200),
IP nvarchar(50) not null,
OperationSystem nvarchar(200),
Memory int,
InventNumber nvarchar(100) not null,
Name nvarchar(1000) not null,
EmployeeId uniqueidentifier foreign key references Employee(Id)
)

Create table Printer(
Id uniqueidentifier primary key,
Name nvarchar(200) not null,
InventNumber nvarchar(200) not null,
Cartridge nvarchar(200),
IP nvarchar(50),
Cabinet nvarchar(100),
DepartmentId uniqueidentifier foreign key references Department(Id),
EmployeeId uniqueidentifier foreign key references Employee(Id)
)

Create table USB(
Id uniqueidentifier primary key,
SerialNumber nvarchar(200) not null,
Size int,
EmployeeId uniqueidentifier foreign key references Employee(Id)
)
