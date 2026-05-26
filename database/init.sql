-- ==============================================
-- LMS Database Initialization Script
-- SQL Server (MSSQL) | sa / 1234567890
-- ==============================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'LMS_DB')
BEGIN
    CREATE DATABASE LMS_DB;
END
GO

USE LMS_DB;
GO

-- ==============================================
-- DROP TABLES (order respects FK constraints)
-- ==============================================
IF OBJECT_ID('dbo.Enrollment', 'U') IS NOT NULL DROP TABLE dbo.Enrollment;
IF OBJECT_ID('dbo.Course',     'U') IS NOT NULL DROP TABLE dbo.Course;
IF OBJECT_ID('dbo.Student',    'U') IS NOT NULL DROP TABLE dbo.Student;
IF OBJECT_ID('dbo.Subject',    'U') IS NOT NULL DROP TABLE dbo.Subject;
IF OBJECT_ID('dbo.Semester',   'U') IS NOT NULL DROP TABLE dbo.Semester;
GO

-- ==============================================
-- CREATE TABLES
-- ==============================================

CREATE TABLE Semester (
    SemesterId   INT            NOT NULL IDENTITY(1,1),
    SemesterName NVARCHAR(100)  NOT NULL,
    StartDate    DATETIME       NOT NULL,
    EndDate      DATETIME       NOT NULL,
    CONSTRAINT PK_Semester PRIMARY KEY (SemesterId)
);

CREATE TABLE Subject (
    SubjectId   INT           NOT NULL IDENTITY(1,1),
    SubjectCode VARCHAR(20)   NOT NULL,
    SubjectName NVARCHAR(100) NOT NULL,
    Credit      INT           NOT NULL,
    CONSTRAINT PK_Subject       PRIMARY KEY (SubjectId),
    CONSTRAINT UQ_SubjectCode   UNIQUE      (SubjectCode)
);

CREATE TABLE Course (
    CourseId   INT           NOT NULL IDENTITY(1,1),
    CourseName NVARCHAR(100) NOT NULL,
    SemesterId INT           NOT NULL,
    SubjectId  INT           NOT NULL,
    CONSTRAINT PK_Course             PRIMARY KEY (CourseId),
    CONSTRAINT FK_Course_Semester    FOREIGN KEY (SemesterId) REFERENCES Semester(SemesterId),
    CONSTRAINT FK_Course_Subject     FOREIGN KEY (SubjectId)  REFERENCES Subject(SubjectId)
);

CREATE TABLE Student (
    StudentId   INT           NOT NULL IDENTITY(1,1),
    FullName    NVARCHAR(100) NOT NULL,
    Email       VARCHAR(100)  NOT NULL,
    DateOfBirth DATETIME      NOT NULL,
    CONSTRAINT PK_Student    PRIMARY KEY (StudentId),
    CONSTRAINT UQ_Email      UNIQUE      (Email)
);

CREATE TABLE Enrollment (
    EnrollmentId INT         NOT NULL IDENTITY(1,1),
    StudentId    INT         NOT NULL,
    CourseId     INT         NOT NULL,
    EnrollDate   DATETIME    NOT NULL,
    Status       VARCHAR(20) NOT NULL DEFAULT 'Active',
    CONSTRAINT PK_Enrollment          PRIMARY KEY (EnrollmentId),
    CONSTRAINT FK_Enrollment_Student  FOREIGN KEY (StudentId) REFERENCES Student(StudentId),
    CONSTRAINT FK_Enrollment_Course   FOREIGN KEY (CourseId)  REFERENCES Course(CourseId)
);
GO

-- ==============================================
-- SEED: 5 SEMESTERS
-- ==============================================
INSERT INTO Semester (SemesterName, StartDate, EndDate) VALUES
    (N'Spring 2023', '2023-01-09', '2023-05-12'),
    (N'Summer 2023', '2023-05-22', '2023-09-01'),
    (N'Fall 2023',   '2023-09-11', '2024-01-12'),
    (N'Spring 2024', '2024-01-08', '2024-05-10'),
    (N'Summer 2024', '2024-05-20', '2024-08-30');
GO

-- ==============================================
-- SEED: 10 SUBJECTS
-- ==============================================
INSERT INTO Subject (SubjectCode, SubjectName, Credit) VALUES
    ('PRN211', N'Basic Cross-Platform Application Programming With .NET',    3),
    ('PRN212', N'OOP with Java',                                              3),
    ('PRN231', N'Advanced Cross-Platform API Programming',                    3),
    ('PRN232', N'REST API Basics and Deployment',                             3),
    ('SWR302', N'Software Requirement',                                       3),
    ('SWD392', N'Software Architecture and Design',                           3),
    ('PRJ301', N'Java Web Application Development',                           3),
    ('IOT102', N'Internet of Things',                                         3),
    ('MAD101', N'Mobile Application Development',                             3),
    ('AIL303', N'Introduction to Artificial Intelligence',                    3);
GO

-- ==============================================
-- SEED: 20 COURSES (4 per semester)
-- ==============================================
INSERT INTO Course (CourseName, SemesterId, SubjectId) VALUES
    -- Spring 2023 (SemesterId=1)
    (N'PRN211 - SP23', 1, 1),
    (N'PRN212 - SP23', 1, 2),
    (N'SWR302 - SP23', 1, 5),
    (N'IOT102 - SP23', 1, 8),
    -- Summer 2023 (SemesterId=2)
    (N'PRN231 - SU23', 2, 3),
    (N'SWD392 - SU23', 2, 6),
    (N'PRJ301 - SU23', 2, 7),
    (N'MAD101 - SU23', 2, 9),
    -- Fall 2023 (SemesterId=3)
    (N'PRN232 - FA23', 3, 4),
    (N'AIL303 - FA23', 3, 10),
    (N'PRN211 - FA23', 3, 1),
    (N'PRJ301 - FA23', 3, 7),
    -- Spring 2024 (SemesterId=4)
    (N'PRN231 - SP24', 4, 3),
    (N'PRN232 - SP24', 4, 4),
    (N'SWR302 - SP24', 4, 5),
    (N'AIL303 - SP24', 4, 10),
    -- Summer 2024 (SemesterId=5)
    (N'PRN212 - SU24', 5, 2),
    (N'SWD392 - SU24', 5, 6),
    (N'MAD101 - SU24', 5, 9),
    (N'IOT102 - SU24', 5, 8);
GO

-- ==============================================
-- SEED: 50 STUDENTS
-- ==============================================
INSERT INTO Student (FullName, Email, DateOfBirth) VALUES
    (N'Nguyen Van An',        'nguyenvanan@student.fpt.edu.vn',       '2002-03-15'),
    (N'Tran Thi Bich',        'tranthhibich@student.fpt.edu.vn',      '2002-07-22'),
    (N'Le Van Cuong',         'levancuong@student.fpt.edu.vn',        '2001-11-05'),
    (N'Pham Thi Dung',        'phamthidung@student.fpt.edu.vn',       '2003-01-18'),
    (N'Hoang Van Em',         'hoangvanem@student.fpt.edu.vn',        '2002-05-30'),
    (N'Nguyen Thi Phuong',    'nguyenthiphuong@student.fpt.edu.vn',   '2001-09-12'),
    (N'Vo Van Gioi',          'vovagioi@student.fpt.edu.vn',          '2002-12-03'),
    (N'Do Thi Hang',          'dothihang@student.fpt.edu.vn',         '2003-04-25'),
    (N'Bui Van Iet',          'buivaniet@student.fpt.edu.vn',         '2001-08-14'),
    (N'Dang Thi Kim',         'dangthikim@student.fpt.edu.vn',        '2002-06-07'),
    (N'Nguyen Quoc Long',     'nguyenquoclong@student.fpt.edu.vn',    '2002-02-19'),
    (N'Tran Van Minh',        'tranvanminh@student.fpt.edu.vn',       '2001-10-28'),
    (N'Le Thi Ngoc',          'lethingoc@student.fpt.edu.vn',         '2003-03-11'),
    (N'Pham Van Oanh',        'phamvanoanh@student.fpt.edu.vn',       '2002-08-09'),
    (N'Hoang Thi Phuong',     'hoangthiphuong@student.fpt.edu.vn',    '2001-12-21'),
    (N'Vo Quang Quy',         'voquangquy@student.fpt.edu.vn',        '2003-05-16'),
    (N'Do Van Rong',          'dovarong@student.fpt.edu.vn',          '2002-01-04'),
    (N'Bui Thi Suong',        'buithisuong@student.fpt.edu.vn',       '2001-07-30'),
    (N'Dang Van Tam',         'dangvantam@student.fpt.edu.vn',        '2002-09-17'),
    (N'Nguyen Thi Uyen',      'nguyenthiuyen@student.fpt.edu.vn',     '2003-11-08'),
    (N'Tran Van Viet',        'tranvanviet@student.fpt.edu.vn',       '2002-04-23'),
    (N'Le Thi Xuan',          'lethixuan@student.fpt.edu.vn',         '2001-06-15'),
    (N'Pham Van Yen',         'phamvanyen@student.fpt.edu.vn',        '2003-02-27'),
    (N'Hoang Quoc Zung',      'hoangquoczung@student.fpt.edu.vn',     '2002-10-01'),
    (N'Vo Thi Anh',           'vothianh@student.fpt.edu.vn',          '2001-03-20'),
    (N'Do Van Binh',          'dovanbinh@student.fpt.edu.vn',         '2003-07-06'),
    (N'Bui Thi Chau',         'buithichau@student.fpt.edu.vn',        '2002-11-13'),
    (N'Dang Van Duc',         'dangvanduc@student.fpt.edu.vn',        '2001-05-24'),
    (N'Nguyen Thi Eo',        'nguyenthieo@student.fpt.edu.vn',       '2003-09-02'),
    (N'Tran Quoc Fanh',       'tranquocfanh@student.fpt.edu.vn',      '2002-01-29'),
    (N'Le Van Giau',          'levangiau@student.fpt.edu.vn',         '2001-04-10'),
    (N'Pham Thi Huong',       'phamthihuong@student.fpt.edu.vn',      '2003-08-18'),
    (N'Hoang Van Inh',        'hoangvaninhk@student.fpt.edu.vn',      '2002-12-05'),
    (N'Vo Thi Jenh',          'vothijenh@student.fpt.edu.vn',         '2001-02-14'),
    (N'Do Quoc Khanh',        'doquockhanh@student.fpt.edu.vn',       '2003-06-22'),
    (N'Bui Van Linh',         'buivanlinh@student.fpt.edu.vn',        '2002-03-08'),
    (N'Dang Thi Mai',         'dangthimai@student.fpt.edu.vn',        '2001-09-26'),
    (N'Nguyen Van Nam',       'nguyenvannam@student.fpt.edu.vn',      '2003-01-14'),
    (N'Tran Thi Oanh',        'tranthioanh@student.fpt.edu.vn',       '2002-07-03'),
    (N'Le Van Phong',         'levanphong@student.fpt.edu.vn',        '2001-11-19'),
    (N'Pham Quoc Quyen',      'phamquocquyen@student.fpt.edu.vn',     '2003-04-07'),
    (N'Hoang Thi Rang',       'hoangthirang@student.fpt.edu.vn',      '2002-08-25'),
    (N'Vo Van Son',           'vovanson@student.fpt.edu.vn',          '2001-12-12'),
    (N'Do Thi Trang',         'dothitrang@student.fpt.edu.vn',        '2003-02-01'),
    (N'Bui Quoc Uy',          'buiquocuy@student.fpt.edu.vn',         '2002-06-20'),
    (N'Dang Van Vuong',       'dangvanvuong@student.fpt.edu.vn',      '2001-10-09'),
    (N'Nguyen Thi Xoan',      'nguyenthixoan@student.fpt.edu.vn',     '2003-03-28'),
    (N'Tran Van Yen',         'tranvanyen@student.fpt.edu.vn',        '2002-05-16'),
    (N'Le Thi Zung',          'lethizung@student.fpt.edu.vn',         '2001-08-04'),
    (N'Pham Van Anh',         'phamvananh@student.fpt.edu.vn',        '2003-12-23');
GO

-- ==============================================
-- SEED: ENROLLMENTS (500 total: 25 per course)
-- ==============================================
DECLARE @cid INT = 1;
WHILE @cid <= 20
BEGIN
    DECLARE @j INT = 0;
    WHILE @j < 25
    BEGIN
        -- Spread students: each course gets 25 unique students (out of 50)
        DECLARE @sid INT = ((@cid * 11 + @j * 2) % 50) + 1;

        DECLARE @status VARCHAR(20) = CASE (@j % 4)
            WHEN 0 THEN 'Active'
            WHEN 1 THEN 'Completed'
            WHEN 2 THEN 'Dropped'
            ELSE        'Active'
        END;

        DECLARE @edate DATE = DATEADD(DAY, (@cid * 13 + @j * 7) % 300, '2023-01-09');

        IF NOT EXISTS (SELECT 1 FROM Enrollment WHERE StudentId = @sid AND CourseId = @cid)
            INSERT INTO Enrollment (StudentId, CourseId, EnrollDate, Status)
            VALUES (@sid, @cid, @edate, @status);

        SET @j = @j + 1;
    END
    SET @cid = @cid + 1;
END
GO

PRINT 'LMS_DB initialized successfully.';
GO
