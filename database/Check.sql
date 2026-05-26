USE LMS_DB;
SELECT
    (SELECT COUNT(*) FROM Semester)   AS [Semesters Count],
    (SELECT COUNT(*) FROM Student)    AS [Students Count],
    (SELECT COUNT(*) FROM Subject)    AS [Subjects Count],
    (SELECT COUNT(*) FROM Course)     AS [Courses Count],
    (SELECT COUNT(*) FROM Enrollment) AS [Enrollments Count];