drop database if exists user_registration;
create Database user_registration;

use user_registration;

drop table if exists tbl_user; 
create table tbl_user (id int primary key identity(1,1),
					firstname varchar(255),
					lastname varchar(255),
					username varchar(255),
					email varchar(255),
					mobileno varchar(10),
					password varchar(255),
					salt varchar(255),
					address text,
					city varchar(255),
					state varchar(255),
					role int default 1,
					is_active tinyint default 1);



drop procedure if exists insert_record; 
go 
create procedure insert_record @fname varchar(255),
								@lname varchar(255),
								@username varchar(255),
								@email varchar(255),
								@mobileno varchar(10),
								@password varchar(255),
								@salt varchar(4),
								@address text,
								@city varchar(255),
								@state varchar(255)
as
	begin
		set @password= (select CONVERT(varchar(32),HASHBYTES('md5',CONCAT(@password,@salt)),2));
		insert into tbl_user(firstname,lastname,username,email,mobileno,password,salt,address,city,state) values 
		(@fname,@lname,@username,@email,@mobileno,@password,@salt,@address,@city,@state); 
	end;
go


exec insert_record 'admin','admin','admin123','admin@gmail.com','9874563210','12345','abcd','baroda','baroda','gujarat';


drop procedure if exists check_user; 
go 
create procedure check_user @username varchar(255),
								@email varchar(255)
as
	begin
		select * from tbl_user where username=@username And email=@email And is_active=1;
	end;
go

exec check_user 'tushar123','tushar@gmail.com';

drop procedure if exists login_user; 
go 
create procedure login_user @username varchar(255),
								@email varchar(255),
								@password varchar(255),
								@salt varchar(4)
as
	begin
		set @password= (select CONVERT(varchar(32),HASHBYTES('md5',CONCAT(@password,@salt)),2));
		select * from tbl_user where username=@username And email=@email and password=@password and is_active=1;
	end;
go

exec login_user 'tushar123','tushar@gmail.com','12345','abcd';

drop procedure if exists show_all_user; 
go 
create procedure show_all_user 
as
	begin
		select * from tbl_user where role=1 and is_active=1;
	end;
go

exec show_all_user;

drop procedure if exists update_record; 
go 
create procedure update_record @fname varchar(255),
								@lname varchar(255),
								@mobileno varchar(10),
								@address text,
								@city varchar(255),
								@state varchar(255),
								@id int
as
	begin
		update tbl_user set firstname=@fname,lastname=@lname,mobileno=@mobileno,
		address=@address,city=@city,state=@state where id=@id;
	end;
go

drop procedure if exists show_one_user; 
go 
create procedure show_one_user @id int
as
	begin
		select * from tbl_user where id=@id And is_active=1;
	end;
go

exec  show_one_user 50;

drop procedure if exists delete_record; 
go 
create procedure delete_record @id int
as
	begin
		update tbl_user set is_active=0 where id=@id and role=1;
	end;
go