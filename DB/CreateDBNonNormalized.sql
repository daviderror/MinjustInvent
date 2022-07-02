Create Table ARMOrder(
Id uniqueidentifier primary key,
Num int unique not null,
Name nvarchar(200),
Segment nvarchar(100),
IpAdress nvarchar(100),
OperationSystem nvarchar(200),
Memory nvarchar(100),
InventNumber nvarchar(100),
ComputerName nvarchar(200),
Services nvarchar,
AccountName nvarchar
)

Create Table PrinterOrder(
Id uniqueidentifier primary key,
CabinetNum int,
Name nvarchar(200),
InventNumber nvarchar(200),
Cartridge nvarchar(200),
IP nvarchar(50)
)

Create Table Department(
Id uniqueidentifier primary key,
Name nvarchar not null,
IndexNum nvarchar(10)
)

Create Table TelephonyOrder(
Id uniqueidentifier primary key,
Num int,
Name nvarchar(200),
CabinetNum int,
Position nvarchar(1000),
CityPhone nvarchar(100),
InternalPhone nvarchar(100),
DepartmentId uniqueidentifier foreign key references Department(Id),
)

Create table USBOrder(
Id uniqueidentifier primary key,
Name nvarchar(200),
SerialNumber nvarchar(200),
Size nvarchar(100),
)

Create table KartochkiOrder(
Id uniqueidentifier primary key,
Name nvarchar(200),
Card nvarchar(100),
ReceivedSignature nvarchar(200),
IssuedSignature nvarchar(200),
DepartmentId uniqueidentifier foreign key references Department(Id),
)