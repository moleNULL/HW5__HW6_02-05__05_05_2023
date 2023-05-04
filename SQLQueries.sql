--------------------------------DDL-----------------------------------
CREATE DATABASE CHI_Homework;
GO

USE CHI_Homework;

CREATE TABLE Groups
(
	gr_id INT IDENTITY PRIMARY KEY,
	gr_name NVARCHAR(50) NOT NULL,
	gr_temp DECIMAL(4, 1) NOT NULL
)

CREATE TABLE Analysis
(
	an_id INT IDENTITY PRIMARY KEY,
	an_name NVARCHAR(100) NOT NULL,
	an_cost MONEY NOT NULL,
	an_price MONEY NOT NULL,
	an_group INT NOT NULL,
	CONSTRAINT FK_Analysis_To_Groups FOREIGN KEY(an_group) REFERENCES Groups(gr_id) ON DELETE CASCADE
)

CREATE TABLE Orders
(
	ord_id INT IDENTITY PRIMARY KEY,
	ord_datetime DATETIME NOT NULL,
	ord_an INT NOT NULL,
	CONSTRAINT FK_Orders_To_Analysis FOREIGN KEY(ord_an) REFERENCES Analysis(an_id) ON DELETE CASCADE
)
GO

--------------------------------DML-----------------------------------

INSERT INTO Groups (gr_name, gr_temp)
VALUES
	('Blood tests', 20.1),
	('Urine tests', 4.5),
	('Stool tests', -15.2),
	('Genetic tests', 18.6),
	('Immunological tests', 20.3),
	('Microbiology tests', 5.0);

INSERT INTO Analysis (an_name, an_cost, an_price, an_group)
VALUES
	('Complete Blood Count', 50.00, 100.00, 1),
    ('Basic Metabolic Panel', 70.00, 150.00, 1),
    ('Urinalysis', 30.00, 80.00, 2),
    ('Urine Culture', 60.00, 120.00, 2),
    ('Fecal Occult Blood Test', 40.00, 90.00, 3),
    ('Stool Culture', 80.00, 150.00, 3),
    ('Cystic Fibrosis Gene Test', 300.00, 600.00, 4),
    ('Hemochromatosis DNA Test', 250.00, 500.00, 4),
    ('Allergy Blood Test', 100.00, 250.00, 5),
    ('ELISA Test', 80.00, 200.00, 5),
    ('Throat Culture', 50.00, 120.00, 6),
    ('Swab Culture', 40.00, 100.00, 6);

INSERT INTO Orders (ord_datetime, ord_an)
VALUES 
    ('2020-01-15 10:30:00', 2),
    ('2020-01-18 12:00:00', 1),
    ('2020-01-19 08:15:00', 3),
    ('2020-02-02 14:30:00', 4),
    ('2020-02-05 16:45:00', 5),
    ('2020-02-06 09:00:00', 6),
    ('2020-02-09 11:30:00', 8),
    ('2020-02-10 13:15:00', 2),
    ('2020-02-10 15:45:00', 9),
    ('2020-02-11 08:30:00', 9),
    ('2020-02-25 10:00:00', 5),
    ('2020-03-10 12:30:00', 6),
    ('2020-03-12 15:00:00', 7),
    ('2020-03-11 11:00:00', 2),
    ('2020-04-09 09:15:00', 3),
    ('2020-04-11 14:00:00', 4),
    ('2020-04-15 16:30:00', 5),
    ('2020-04-22 08:45:00', 6),
    ('2020-04-29 10:30:00', 1),
    ('2020-05-04 12:15:00', 9),
    ('2020-05-06 15:30:00', 5),
    ('2020-05-10 09:00:00', 4),
    ('2020-05-31 11:45:00', 5),
    ('2020-06-15 13:30:00', 6),
    ('2020-06-20 16:15:00', 1),
    ('2020-06-28 08:30:00', 2),
    ('2020-07-15 10:45:00', 3),
    ('2020-07-25 14:00:00', 4),
    ('2020-08-24 12:15:00', 9),
    ('2020-08-29 15:30:00', 6),
    ('2020-08-31 09:00:00', 10),
    ('2020-09-10 11:30:00', 10),
    ('2020-09-19 13:45:00', 12),
    ('2020-10-18 16:00:00', 11),
    ('2020-10-20 08:15:00', 12),
    ('2020-10-25 11:15:00', 2),
    ('2020-11-10 09:15:00', 9),
    ('2020-11-20 15:15:00', 7),
    ('2020-11-28 18:15:00', 5),
    ('2020-12-02 09:00:00', 3),
    ('2020-12-05 08:15:00', 11),
    ('2020-12-19 06:15:00', 5),
    ('2020-12-25 11:15:00', 4),
    ('2021-01-08 16:05:00', 1),
    ('2021-01-11 09:15:00', 10),
    ('2021-01-15 10:15:00', 8),
    ('2021-01-20 15:15:00', 6),
    ('2021-02-24 04:15:00', 4),
    ('2021-02-25 08:00:00', 3),
    ('2021-02-28 18:20:00', 11)
GO

------------------------------Task_1----------------------------------	
	
SELECT an_name AS Analysis_Name, an_price AS Analysis_Price FROM Analysis
INNER JOIN Orders ON an_id = ord_an
WHERE ord_datetime BETWEEN '2020-02-05' AND '2020-02-12'

------------------------------Task_2----------------------------------	

SELECT  gr_name AS Analysis_Group,
		YEAR(o.ord_datetime) AS Order_Year,
		MONTH(o.ord_datetime) AS Order_Month, 
		Count(*) AS Tests_Sold FROM Orders o
INNER JOIN Analysis a ON a.an_id = o.ord_an
INNER JOIN Groups g ON a.an_group = g.gr_id
GROUP BY g.gr_name, YEAR(o.ord_datetime), MONTH(o.ord_datetime)