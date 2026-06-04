create database HospitalDB

use HospitalDB

CREATE TABLE Patients(
    id INT IDENTITY(1000,1) PRIMARY KEY,
    fullName VARCHAR(100) NOT NULL,
    dob DATE NOT NULL,
    gender VARCHAR(10) NOT NULL
        CHECK (gender IN ('Male', 'Female', 'Other')),
    mob VARCHAR(15) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE,
    status VARCHAR(20) DEFAULT 'Active'
        CHECK (status IN ('Active', 'Inactive'))
);

CREATE TABLE Doctors(
    id INT IDENTITY(100,1) PRIMARY KEY,
    fullName VARCHAR(100) NOT NULL,
    specialization VARCHAR(100) NOT NULL,
    mob VARCHAR(15) UNIQUE NOT NULL,
    fee INT CHECK (fee >= 0),
    available BIT DEFAULT 1
);


CREATE TABLE Appointments(
    appointmentId INT IDENTITY PRIMARY KEY,
    patientId INT NOT NULL,
    doctorId INT NOT NULL,
    appointmentDate DATETIME NOT NULL,
    status VARCHAR(20) DEFAULT 'Scheduled'
        CHECK (status IN ('Scheduled', 'Completed', 'Cancelled')),

    FOREIGN KEY (patientId) REFERENCES Patients(id)
        ON DELETE CASCADE,

    FOREIGN KEY (doctorId) REFERENCES Doctors(id)
        ON DELETE CASCADE
);

INSERT INTO Patients (fullName, dob, gender, mob, email, status) VALUES
('Amit Sharma', '1995-06-15', 'Male', '9876543210', 'amit@gmail.com', 'Active'),
('Priya Verma', '1998-03-22', 'Female', '9876543211', 'priya@gmail.com', 'Active'),
('Rahul Singh', '1992-11-10', 'Male', '9876543212', 'rahul@gmail.com', 'Inactive'),
('Sneha Reddy', '2000-01-05', 'Female', '9876543213', 'sneha@gmail.com', 'Active'),
('Arjun Mehta', '1989-09-18', 'Male', '9876543214', 'arjun@gmail.com', 'Active'),
('Pooja Nair', '1996-07-25', 'Female', '9876543215', 'pooja@gmail.com', 'Active'),
('Karan Patel', '1993-04-12', 'Male', '9876543216', 'karan@gmail.com', 'Inactive'),
('Neha Gupta', '1999-12-30', 'Female', '9876543217', 'neha@gmail.com', 'Active'),
('Rohit Kumar', '1991-02-14', 'Male', '9876543218', 'rohit@gmail.com', 'Active'),
('Anjali Das', '1997-08-08', 'Female', '9876543219', 'anjali@gmail.com', 'Active');

INSERT INTO Doctors (fullName, specialization, mob, fee, available) VALUES
('Dr. Raj Malhotra', 'Cardiologist', '9000000001', 800, 1),
('Dr. Sunita Rao', 'Dermatologist', '9000000002', 500, 1),
('Dr. Vivek Sharma', 'Orthopedic', '9000000003', 700, 1),
('Dr. Kavita Iyer', 'Pediatrician', '9000000004', 600, 1),
('Dr. Aman Verma', 'Neurologist', '9000000005', 1000, 1),
('Dr. Meera Joshi', 'Gynecologist', '9000000006', 750, 1),
('Dr. Rohit Gupta', 'ENT Specialist', '9000000007', 400, 0),
('Dr. Anil Kapoor', 'General Physician', '9000000008', 300, 1),
('Dr. Suresh Yadav', 'Urologist', '9000000009', 900, 1),
('Dr. Nisha Singh', 'Psychiatrist', '9000000010', 850, 1);
   

INSERT INTO Appointments (patientId, doctorId, appointmentDate, status) VALUES
(1000, 100, '2026-06-01 10:00:00', 'Scheduled'),
(1001, 101, '2026-06-01 11:00:00', 'Completed'),
(1002, 102, '2026-06-02 09:30:00', 'Cancelled'),
(1003, 103, '2026-06-02 12:00:00', 'Scheduled'),
(1004, 104, '2026-06-03 10:30:00', 'Completed'),
(1005, 105, '2026-06-03 01:00:00', 'Scheduled'),
(1006, 106, '2026-06-04 11:15:00', 'Cancelled'),
(1007, 107, '2026-06-04 02:00:00', 'Completed'),
(1008, 108, '2026-06-05 09:00:00', 'Scheduled'),
(1009, 109, '2026-06-05 03:00:00', 'Scheduled'),

(1000, 101, '2026-06-06 10:00:00', 'Completed'),
(1001, 102, '2026-06-06 11:30:00', 'Scheduled'),
(1002, 103, '2026-06-07 09:45:00', 'Cancelled'),
(1003, 104, '2026-06-07 12:30:00', 'Scheduled'),
(1004, 105, '2026-06-08 10:15:00', 'Completed'),
(1005, 106, '2026-06-08 01:30:00', 'Scheduled'),
(1006, 107, '2026-06-09 11:45:00', 'Cancelled'),
(1007, 108, '2026-06-09 02:15:00', 'Completed'),
(1008, 109, '2026-06-10 09:30:00', 'Scheduled'),
(1009, 100, '2026-06-10 03:30:00', 'Scheduled'),

(1000, 102, '2026-06-11 10:00:00', 'Completed'),
(1001, 103, '2026-06-11 11:00:00', 'Scheduled'),
(1002, 104, '2026-06-12 09:30:00', 'Cancelled'),
(1003, 105, '2026-06-12 12:00:00', 'Scheduled'),
(1004, 106, '2026-06-13 10:30:00', 'Completed'),
(1005, 107, '2026-06-13 01:00:00', 'Scheduled'),
(1006, 108, '2026-06-14 11:15:00', 'Cancelled'),
(1007, 109, '2026-06-14 02:00:00', 'Completed'),
(1008, 100, '2026-06-15 09:00:00', 'Scheduled'),
(1009, 101, '2026-06-15 03:00:00', 'Scheduled');

select * from patients
select * from doctors
select * from Appointments

-- Stored procedures on Patient table

 ALTER PROCEDURE sp_AddPatient
    @fullName VARCHAR(100), 
    @dob DATE, 
    @gender VARCHAR(10), 
    @mob VARCHAR(15), 
    @email VARCHAR(100), 
    @status VARCHAR(20) = 'Active'
AS
BEGIN
    SET NOCOUNT ON; 

    BEGIN TRY

        IF @gender NOT IN ('Male', 'Female', 'Other')
        BEGIN
            THROW 50001, 'Invalid Gender Value', 1;
        END

        INSERT INTO Patients (fullName, dob, gender, mob, email, status)
        VALUES (@fullName, @dob, @gender, @mob, @email, @status);

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END


EXEC sp_AddPatient
    @fullName = 'Test User 12',
    @dob = '2001-05-10',
    @gender = 'Male',
    @mob = '9990000501',
    @email = 'test2@gmail.com';

ALTER PROCEDURE sp_UpdatePatient
    @id INT,
    @fullName VARCHAR(100), 
    @dob DATE, 
    @gender VARCHAR(10), 
    @mob VARCHAR(15), 
    @email VARCHAR(100) = NULL,
    @status VARCHAR(20) = 'Active'
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        -- Check if patient exists
        IF NOT EXISTS (SELECT 1 FROM Patients WHERE id = @id)
            THROW 50001, 'Patient not found', 1;

        -- Validate gender
        IF @gender NOT IN ('Male', 'Female', 'Other')
            THROW 50002, 'Invalid gender value', 1;

        -- Check duplicate mobile (excluding current patient)
        IF EXISTS (SELECT 1 FROM Patients WHERE mob = @mob AND id <> @id)
            THROW 50003, 'Mobile number already exists', 1;

        -- Check duplicate email (excluding current patient)
        IF @email IS NOT NULL AND EXISTS 
        (
            SELECT 1 FROM Patients WHERE email = @email AND id <> @id
        )
            THROW 50004, 'Email already exists', 1;

        -- Update patient
        UPDATE Patients
        SET 
            fullName = @fullName,
            dob = @dob,
            gender = @gender,
            mob = @mob,
            email = @email,
            status = @status
        WHERE id = @id;

    END TRY
    BEGIN CATCH
        THROW; -- pass error to application
    END CATCH
END

EXEC sp_UpdatePatient
    @id = 1000,
    @fullName = 'Amit raj Sharma',
    @dob = '1995-06-15',
    @gender = 'Male',
    @mob = '9876543210',
    @email = 'amit_updated@gmail.com',
    @status = 'Active';

CREATE PROCEDURE sp_DeactivatePatient
    @id INT
AS
BEGIN
    BEGIN TRY

        -- Check if patient exists
        IF NOT EXISTS (SELECT 1 FROM Patients WHERE id = @id)
        BEGIN
            THROW 50004, 'Patient not found', 1;
        END

        -- Deactivate patient
        UPDATE Patients
        SET status = 'Inactive'
        WHERE id = @id;

        PRINT 'Patient deactivated successfully';

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END

CREATE PROCEDURE sp_GetActivePatients
AS
BEGIN
    SELECT *
    FROM Patients
    WHERE status = 'Active';
END

EXEC sp_DeactivatePatient @id = 1000;
SELECT * FROM Patients WHERE id = 1000;
EXEC sp_DeactivatePatient @id = 9999;
EXEC sp_GetActivePatients;
SELECT * FROM Patients WHERE status = 'Active';


CREATE PROCEDURE sp_GetDoctorsByFilter
    @specialization VARCHAR(100) = NULL,
    @available BIT = NULL
AS
BEGIN
    SELECT *
    FROM Doctors
    WHERE
        (@specialization IS NULL OR LOWER(specialization) = LOWER(@specialization))
        AND
        (@available IS NULL OR available = @available);
END

EXEC sp_GetDoctorsByFilter;
EXEC sp_GetDoctorsByFilter @specialization = 'Cardiologist';
EXEC sp_GetDoctorsByFilter @available = 1;
EXEC sp_GetDoctorsByFilter @available = 0;
EXEC sp_GetDoctorsByFilter
    @specialization = 'Cardiologist',
    @available = 1;
EXEC sp_GetDoctorsByFilter
    @specialization = 'Dentist',
    @available = 0;


ALTER PROCEDURE sp_AddAppointment
    @patientId INT,
    @doctorId INT,
    @appointmentDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        BEGIN TRAN;

        -- ❗ Validate Patient
        IF NOT EXISTS (SELECT 1 FROM Patients WHERE id = @patientId)
            THROW 50010, 'Patient not found', 1;

        -- ❗ Validate Doctor
        IF NOT EXISTS (SELECT 1 FROM Doctors WHERE id = @doctorId)
            THROW 50011, 'Doctor not found', 1;

        -- ❗ Doctor Availability
        IF EXISTS (SELECT 1 FROM Doctors WHERE id = @doctorId AND available = 0)
            THROW 50012, 'Doctor not available', 1;

        -- ❗ Prevent duplicate booking
        IF EXISTS (
            SELECT 1 FROM Appointments
            WHERE doctorId = @doctorId
              AND appointmentDate = @appointmentDate
              AND status = 'Scheduled'
        )
            THROW 50015, 'Doctor already booked for this slot', 1;

        INSERT INTO Appointments (patientId, doctorId, appointmentDate, status)
        VALUES (@patientId, @doctorId, @appointmentDate, 'Scheduled');

        COMMIT;

    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW; 
    END CATCH
END

ALTER TABLE Appointments
ADD cancelledAt DATETIME NULL;


ALTER PROCEDURE sp_CancelAppointment
    @appointmentId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM Appointments WHERE appointmentId = @appointmentId)
            THROW 50013, 'Appointment not found', 1;

        IF EXISTS (
            SELECT 1 FROM Appointments
            WHERE appointmentId = @appointmentId AND status <> 'Scheduled'
        )
            THROW 50014, 'Only scheduled appointments can be cancelled', 1;

        UPDATE Appointments
        SET status = 'Cancelled',
            cancelledAt = GETDATE()
        WHERE appointmentId = @appointmentId;

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END

CREATE PROCEDURE sp_GetUpcomingAppointments
AS
BEGIN
    SELECT *
    FROM Appointments
    WHERE appointmentDate > GETDATE()
      AND status = 'Scheduled';
END

CREATE PROCEDURE sp_GetAppointmentsByDoctor
    @doctorId INT
AS
BEGIN
    SELECT *
    FROM Appointments
    WHERE doctorId = @doctorId;
END

-- testing
EXEC sp_AddAppointment
    @patientId = 1000,
    @doctorId = 100,
    @appointmentDate = '2026-06-20 10:00:00';


UPDATE Doctors SET available = 0 WHERE id = 100;

EXEC sp_CancelAppointment @appointmentId = 1;
EXEC sp_GetUpcomingAppointments;
EXEC sp_GetAppointmentsByDoctor @doctorId = 100;

-- consolidated view

-- Get Appointment details
ALTER PROCEDURE sp_GetAppointmentDetails
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.fullName AS PatientName,
        d.fullName AS DoctorName,
        d.specialization AS Specialization,
        a.appointmentDate AS AppointmentDate,
        a.status AS Status,
        ISNULL(d.fee, 0) AS Fee
    FROM Appointments a
    JOIN Patients p ON a.patientId = p.id
    JOIN Doctors d ON a.doctorId = d.id;
END

-- Total Appointments > 2
CREATE PROCEDURE sp_GetDoctorsWithMoreAppointments
AS
BEGIN
    SELECT d.fullName, COUNT(*) AS totalAppointments
    FROM Appointments a
    JOIN Doctors d ON a.doctorId = d.id
    GROUP BY d.fullName
    HAVING COUNT(*) > 2;
END

-- Revenue by specialization

CREATE PROCEDURE sp_GetRevenueBySpecialization
AS
BEGIN
    SELECT d.specialization, SUM(d.fee) AS totalRevenue
    FROM Appointments a
    JOIN Doctors d ON a.doctorId = d.id
    WHERE a.status = 'Completed'
    GROUP BY d.specialization;
END

-- same patient, same doctor, same date

CREATE PROCEDURE sp_GetDuplicateAppointments
AS
BEGIN
    SELECT 
        patientId, 
        doctorId, 
        CAST(appointmentDate AS DATE) AS AppointmentDay, 
        COUNT(*) AS total
    FROM Appointments
    GROUP BY patientId, doctorId, CAST(appointmentDate AS DATE)
    HAVING COUNT(*) > 1;
END

-- Next 7days Appointments

CREATE PROCEDURE sp_GetNext7DaysAppointments
AS
BEGIN
    SELECT
        appointmentId,
        FORMAT(appointmentDate, 'dd-MMM-yyyy hh:mm tt') AS AppointmentDate,
        status
    FROM Appointments
    WHERE appointmentDate BETWEEN GETDATE() AND DATEADD(DAY, 7, GETDATE());
END


-- Testing

EXEC sp_GetAppointmentDetails;
EXEC sp_GetDoctorsWithMoreAppointments;
EXEC sp_GetRevenueBySpecialization;
EXEC sp_GetDuplicateAppointments;
EXEC sp_GetNext7DaysAppointments;

-- indexes for fast retrival
CREATE INDEX idx_Appointments_DoctorId
ON Appointments(doctorId);

CREATE INDEX idx_Appointments_AppointmentDate
ON Appointments(appointmentDate);

select * from patients
select * from doctors
select * from Appointments