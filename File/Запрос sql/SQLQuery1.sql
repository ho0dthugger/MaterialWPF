-- ������� �������������
CREATE TABLE ������������ (
    ID_������������ INT PRIMARY KEY IDENTITY,
    ����� VARCHAR(50) NOT NULL,
    ������ VARCHAR(50) NOT NULL,
    ���� VARCHAR(50) NOT NULL -- '�������������' ��� '��������'
);
GO

-- ������� ����������
CREATE TABLE ��������� (
    ID_��������� INT PRIMARY KEY IDENTITY,
    �������� VARCHAR(100) NOT NULL,
    ���������� FLOAT NOT NULL
);
GO

-- ������� ���������
CREATE TABLE ��������� (
    ID_��������� INT PRIMARY KEY IDENTITY,
    �������� VARCHAR(100) NOT NULL,
    ����������� VARBINARY(MAX) -- ������ ����������� ���������
);
GO

-- ������� ����� ���������� � ���������
CREATE TABLE ������������������ (
    ID_��������� INT,
    ID_��������� INT,
    ���������� FLOAT NOT NULL,
    PRIMARY KEY (ID_���������, ID_���������),
    FOREIGN KEY (ID_���������) REFERENCES ���������(ID_���������),
    FOREIGN KEY (ID_���������) REFERENCES ���������(ID_���������)
);
GO

-- ������� �������
CREATE TABLE ������ (
    ID_������ INT PRIMARY KEY IDENTITY,
    ID_��������� INT NOT NULL,
    ���������� FLOAT NOT NULL,
    ����_������ DATETIME NOT NULL,
    FOREIGN KEY (ID_���������) REFERENCES ���������(ID_���������)
);
GO

-- ������� ������������
CREATE TABLE ������������ (
    ID_������������ INT PRIMARY KEY IDENTITY,
    ID_��������� INT NOT NULL,
    ������ VARCHAR(100) NOT NULL, -- '� ������������', '������', '����'
    ����_������ DATETIME NOT NULL,
    ����_���������� DATETIME,
    FOREIGN KEY (ID_���������) REFERENCES ���������(ID_���������)
);
GO

-- ������� �����
CREATE TABLE ���� (
    ID_����� INT PRIMARY KEY IDENTITY,
    ID_������������ INT NOT NULL,
    �������� VARCHAR(MAX),
    FOREIGN KEY (ID_������������) REFERENCES ������������(ID_������������)
);
GO

-- ������� ����������
CREATE TABLE ��������� (
    ID_��������� INT PRIMARY KEY IDENTITY,
    ��� VARCHAR(100) NOT NULL,
    ����� VARCHAR(200) NOT NULL
);
GO

-- ������� ����� ������� � ����������
CREATE TABLE ���������������� (
    ID_������ INT,
    ID_��������� INT,
    PRIMARY KEY (ID_������, ID_���������),
    FOREIGN KEY (ID_������) REFERENCES ������(ID_������),
    FOREIGN KEY (ID_���������) REFERENCES ���������(ID_���������)
);
GO

-- ������� �������� ������
-- ������������
INSERT INTO ������������ (�����, ������, ����) VALUES
('admin', 'admin123', '�������������'),
('worker', 'worker123', '��������');

-- ���������
INSERT INTO ��������� (��������, ����������) VALUES
('�����', 500.0),
('������', 300.0),
('�������', 200.0);

-- ���������
INSERT INTO ��������� (��������, �����������) VALUES
('������', NULL), -- ����� ������ ���� ���� � ����������� ��� ���� ����������� � �������� �������
('����������', NULL),
('�����', NULL);

-- ������������������
INSERT INTO ������������������ (ID_���������, ID_���������, ����������) VALUES
(1, 1, 10.0), -- ������ requires 10 units of Steel
(2, 1, 20.0), -- ���������� requires 20 units of Steel
(3, 2, 15.0); -- ����� requires 15 units of Wood

-- ������
INSERT INTO ������ (ID_���������, ����������, ����_������) VALUES
(1, 50, '2023-10-01'),
(2, 30, '2023-10-02');

-- ���������
INSERT INTO ��������� (���, �����) VALUES
('�������� �', '����� �, ����� 1'),
('�������� �', '����� �, ����� 2');

-- ����������������
INSERT INTO ���������������� (ID_������, ID_���������) VALUES
(1, 1),
(2, 2);

-- ������������
INSERT INTO ������������ (ID_���������, ������, ����_������, ����_����������) VALUES
(1, '������', '2023-10-01', '2023-10-02'),
(2, '� ������������', '2023-10-03', NULL);

-- ����
INSERT INTO ���� (ID_������������, ��������) VALUES
(1, '���������� �����');