CREATE TABLE Category (
    id INT NOT NULL PRIMARY KEY IDENTITY,
    category VARCHAR (100) NOT NULL UNIQUE,
    capacity INT NOT NULL,
    description VARCHAR(100) NOT NULL
);

INSERT INTO Category (category,capacity,description)
VALUES
('Alimentação', '15', 'Atenção à validade dos produtos nesta categoria'),
('casa', '2333', 'jflidbwscvhbaawervava'),
('Carro', '22', 'ciapdshbcvpuiaghdc');

CREATE TABLE Feiras (
    id INT NOT NULL PRIMARY KEY IDENTITY,
    nome VARCHAR(100) NOT NULL,
    categoryid INT FOREIGN KEY REFERENCES Category(id),
    numSlots INT NOT NULL,
    descri VARCHAR(100) NOT NULL
);

INSERT INTO Feiras (nome,categoryid,numSlots,descri)
VALUES
('Feira de Fruta V.N.Famalicão', '1','21','Atenção à validade dos produtos nesta categoria'),
('Feira do pão Povoa De Varzim', '1','21', 'jflidbwscvhbaawervava'),
('Carro', '1','21', 'ciapdshbcvpuiaghdc');


CREATE TABLE Utilizadores (
    id INT NOT NULL PRIMARY KEY IDENTITY,
    nome VARCHAR (100) NOT NULL,
    email VARCHAR (100) NOT NULL UNIQUE,
    pass VARCHAR(100) NOT NULL,
    tipo INT NOT NULL
);

INSERT INTO Utilizadores (nome,email,pass,tipo)
VALUES
('Ivo Ribeiro', 'a96726@alunos.uminho.pt', 'IvoRibeiro09','0'),
('Gonçalo Bastos', 'goncalo22@gmail.com', '22gon22','1'),
('Armando Costa', 'armCosta@gmail.com', 'ciapdshbcvpuiaghdc','1');

CREATE TABLE Stand (
    id INT NOT NULL PRIMARY KEY IDENTITY,
    nome VARCHAR (100) NOT NULL,
    vendedorId INT FOREIGN KEY REFERENCES Utilizadores(id),
    feiraId INT FOREIGN KEY REFERENCES Feiras(id)
);

INSERT INTO Stand (nome,vendedorId,feiraId)
VALUES
('Frutismulti', '1', '1'),
('poupaCasa', '2', '2'),
('Carplus', '2', '2');


CREATE TABLE Produtos (
    id INT NOT NULL PRIMARY KEY IDENTITY,
    nome VARCHAR (100) NOT NULL,
    quantidade INT NOT NULL,
    price DECIMAL(4,2) NOT NULL,
    imagelink VARCHAR(300) NOT NULL, 
    standId INT FOREIGN KEY REFERENCES Stand(id),
    idVendedor INT FOREIGN KEY REFERENCES Utilizadores(id),
    descri VARCHAR(100) NOT NULL
);

INSERT INTO Produtos (nome,quantidade,price,imagelink,standId,idVendedor,descri)
VALUES
('Banana de Madeira', '150', '0.9','https://www.imagensempng.com.br/wp-content/uploads/2021/08/Banana-Png-1024x1024.png','1','2','validade ate 19/12/2022'),
('Maçã', '130', '0.45','https://pin.it/dYWNXQ4','1','2','validade ate 10/12/2022'),
('Pera', '100', '0.95','https://pin.it/TTwxV94','1','2','validade ate 21/12/2022');
CREATE TABLE Negociados(
    id INT NOT NULL PRIMARY KEY IDENTITY,
    nome VARCHAR (100) NOT NULL,
    quantidade INT NOT NULL,
    price DECIMAL(4,2) NOT NULL,
    standId INT FOREIGN KEY REFERENCES Stand(id),
    compradorId INT FOREIGN KEY REFERENCES Utilizadores(id)
);

INSERT INTO Negociados (nome,quantidade,price,standId,compradorId)
VALUES
('Banana de Madeira', '150', '0.9','1','2'),
('Maçã', '130', '0.45','1','2'),
('Pera', '100', '0.95','1','2');


CREATE TABLE Carrinho (
    id INT NOT NULL PRIMARY KEY IDENTITY,
    nome VARCHAR (100) NOT NULL,
    idVendedor INT FOREIGN KEY REFERENCES Utilizadores(id),
    idComprador INT FOREIGN KEY REFERENCES Utilizadores(id),
    quantidade INT NOT NULL,
    price DECIMAL(4,2) NOT NULL,
    statu INT NOT NULL
);

INSERT INTO Carrinho (nome,idVendedor,idComprador,quantidade,price,statu)
VALUES
('Banana de Madeira', '2', '4','3','0.8','4'),
('Maçã', '2', '3','2','1.3','2'),
('Pera', '2', '3','5','0.3','4');