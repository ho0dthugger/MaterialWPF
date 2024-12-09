-- Таблица пользователей
CREATE TABLE Пользователи (
    ID_Пользователя INT PRIMARY KEY IDENTITY,
    Логин VARCHAR(50) NOT NULL,
    Пароль VARCHAR(50) NOT NULL,
    Роль VARCHAR(50) NOT NULL -- 'Администратор' или 'Работник'
);
GO

-- Таблица материалов
CREATE TABLE Материалы (
    ID_Материала INT PRIMARY KEY IDENTITY,
    Название VARCHAR(100) NOT NULL,
    Количество FLOAT NOT NULL
);
GO

-- Таблица заготовок
CREATE TABLE Заготовки (
    ID_Заготовки INT PRIMARY KEY IDENTITY,
    Название VARCHAR(100) NOT NULL,
    Изображение VARBINARY(MAX) -- Хранит изображение заготовки
);
GO

-- Таблица связи материалов и заготовок
CREATE TABLE МатериалыЗаготовок (
    ID_Заготовки INT,
    ID_Материала INT,
    Количество FLOAT NOT NULL,
    PRIMARY KEY (ID_Заготовки, ID_Материала),
    FOREIGN KEY (ID_Заготовки) REFERENCES Заготовки(ID_Заготовки),
    FOREIGN KEY (ID_Материала) REFERENCES Материалы(ID_Материала)
);
GO

-- Таблица заказов
CREATE TABLE Заказы (
    ID_Заказа INT PRIMARY KEY IDENTITY,
    ID_Заготовки INT NOT NULL,
    Количество FLOAT NOT NULL,
    Дата_Заказа DATETIME NOT NULL,
    FOREIGN KEY (ID_Заготовки) REFERENCES Заготовки(ID_Заготовки)
);
GO

-- Таблица производства
CREATE TABLE Производство (
    ID_Производства INT PRIMARY KEY IDENTITY,
    ID_Заготовки INT NOT NULL,
    Статус VARCHAR(100) NOT NULL, -- 'В производстве', 'Готово', 'Брак'
    Дата_Начала DATETIME NOT NULL,
    Дата_Завершения DATETIME,
    FOREIGN KEY (ID_Заготовки) REFERENCES Заготовки(ID_Заготовки)
);
GO

-- Таблица брака
CREATE TABLE Брак (
    ID_Брака INT PRIMARY KEY IDENTITY,
    ID_Производства INT NOT NULL,
    Описание VARCHAR(MAX),
    FOREIGN KEY (ID_Производства) REFERENCES Производство(ID_Производства)
);
GO

-- Таблица заказчиков
CREATE TABLE Заказчики (
    ID_Заказчика INT PRIMARY KEY IDENTITY,
    Имя VARCHAR(100) NOT NULL,
    Адрес VARCHAR(200) NOT NULL
);
GO

-- Таблица связи заказов и заказчиков
CREATE TABLE ЗаказыЗаказчиков (
    ID_Заказа INT,
    ID_Заказчика INT,
    PRIMARY KEY (ID_Заказа, ID_Заказчика),
    FOREIGN KEY (ID_Заказа) REFERENCES Заказы(ID_Заказа),
    FOREIGN KEY (ID_Заказчика) REFERENCES Заказчики(ID_Заказчика)
);
GO

-- Вставка образцов данных
-- Пользователи
INSERT INTO Пользователи (Логин, Пароль, Роль) VALUES
('admin', 'admin123', 'Администратор'),
('worker', 'worker123', 'Работник');

-- Материалы
INSERT INTO Материалы (Название, Количество) VALUES
('Сталь', 500.0),
('Дерево', 300.0),
('Пластик', 200.0);

-- Заготовки
INSERT INTO Заготовки (Название, Изображение) VALUES
('Гаечка', NULL), -- Здесь должен быть путь к изображению или само изображение в бинарном формате
('Шестеренка', NULL),
('Рычаг', NULL);

-- МатериалыЗаготовок
INSERT INTO МатериалыЗаготовок (ID_Заготовки, ID_Материала, Количество) VALUES
(1, 1, 10.0), -- Гаечка requires 10 units of Steel
(2, 1, 20.0), -- Шестеренка requires 20 units of Steel
(3, 2, 15.0); -- Рычаг requires 15 units of Wood

-- Заказы
INSERT INTO Заказы (ID_Заготовки, Количество, Дата_Заказа) VALUES
(1, 50, '2023-10-01'),
(2, 30, '2023-10-02');

-- Заказчики
INSERT INTO Заказчики (Имя, Адрес) VALUES
('Компания А', 'Город А, Улица 1'),
('Компания Б', 'Город Б, Улица 2');

-- ЗаказыЗаказчиков
INSERT INTO ЗаказыЗаказчиков (ID_Заказа, ID_Заказчика) VALUES
(1, 1),
(2, 2);

-- Производство
INSERT INTO Производство (ID_Заготовки, Статус, Дата_Начала, Дата_Завершения) VALUES
(1, 'Готово', '2023-10-01', '2023-10-02'),
(2, 'В производстве', '2023-10-03', NULL);

-- Брак
INSERT INTO Брак (ID_Производства, Описание) VALUES
(1, 'Повреждена гайка');