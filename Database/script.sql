-- 1) Bazanı yarat
IF DB_ID('ToDoDb') IS NULL CREATE DATABASE ToDoDb;
GO
USE ToDoDb;
GO

-- 2) Users cədvəli (daxili register yoxdur, yalnız Google ilə girən istifadəçilər)
CREATE TABLE Users (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    GoogleId  NVARCHAR(100) NOT NULL UNIQUE,
    Email     NVARCHAR(200) NOT NULL,
    FullName  NVARCHAR(200) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- 3) Tasks cədvəli. Status: 0 = Todo, 1 = InProgress, 2 = Done
CREATE TABLE Tasks (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    UserId      INT NOT NULL,
    Title       NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NULL,
    Status      TINYINT NOT NULL DEFAULT 0 CHECK (Status IN (0,1,2)),
    CreatedAt   DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Tasks_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
);
