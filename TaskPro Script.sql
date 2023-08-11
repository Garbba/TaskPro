/*CREATE DATABASE TaskPro*/

DROP TABLE IF EXISTS memberlist;

-- Drop the timetrack table
DROP TABLE IF EXISTS timetrack;

-- Drop the commentUser table
DROP TABLE IF EXISTS commentUser;

-- Drop the attachment table
DROP TABLE IF EXISTS attachment;

-- Drop the tasktag table
DROP TABLE IF EXISTS tasktag;

-- Drop the tag table
DROP TABLE IF EXISTS tag;

-- Drop the task table
DROP TABLE IF EXISTS task;

-- Drop the listacess table
DROP TABLE IF EXISTS listacess;

-- Drop the list table
DROP TABLE IF EXISTS list;

-- Drop the userlist table
DROP TABLE IF EXISTS userlist;


CREATE TABLE userlist (
    id        INTEGER PRIMARY KEY IDENTITY,
    nickname  VARCHAR(30) NOT NULL UNIQUE,
    username      VARCHAR(30) NOT NULL,
    lastname  VARCHAR(30) NOT NULL,
    email     VARCHAR(40) NOT NULL UNIQUE,
    userpassword  VARCHAR(25) NOT NULL
);


CREATE TABLE list (
    id    INTEGER PRIMARY KEY IDENTITY,
    listName  VARCHAR(30) NOT NULL
);

CREATE TABLE listacess (
    accesstype  VARCHAR(30) NOT NULL CHECK (accesstype IN ('OWNER', 'ADMIN', 'MEMBER')),
    user_id     INTEGER NOT NULL,
    list_id     INTEGER NOT NULL,
    PRIMARY KEY (user_id, list_id),
    FOREIGN KEY (user_id) REFERENCES UserList (id) ON DELETE CASCADE,
    FOREIGN KEY (list_id) REFERENCES list (id) ON DELETE CASCADE
);


CREATE TABLE task (
    id				 INTEGER PRIMARY KEY IDENTITY,
    title			 VARCHAR(100) NOT NULL,
    taskdescription  VARCHAR(500),
    taskStatus       VARCHAR(15) NOT NULL CHECK (taskStatus IN ('NOT STARTED', 'IN PROGRESS', 'COMPLETED')),
    isfavorite       TINYINT NOT NULL CHECK (isfavorite IN (0, 1)),
    isonmyday        TINYINT NOT NULL CHECK (isonmyday IN (0, 1)),
    startdate		DATE,
    enddate		DATE,
    taskPriority     VARCHAR(15) NOT NULL,
    list_id			INTEGER NOT NULL FOREIGN KEY ( list_id ) REFERENCES list ( id )
);



CREATE TABLE tag (
    id       INTEGER PRIMARY KEY IDENTITY,
    tagName     VARCHAR(30) NOT NULL,
    list_id  INTEGER NOT NULL FOREIGN KEY ( list_id ) REFERENCES list ( id )
);



CREATE TABLE tasktag (
    id       INTEGER PRIMARY KEY IDENTITY,
    tag_id   INTEGER NOT NULL FOREIGN KEY ( tag_id ) REFERENCES tag ( id ) ON DELETE CASCADE,
    task_id  INTEGER NOT NULL FOREIGN KEY ( task_id ) REFERENCES task ( id) ON DELETE CASCADE
);



CREATE TABLE attachment (
    id						INTEGER PRIMARY KEY IDENTITY,
    datefile				DATE NOT NULL,
    attachmentFilename		VARCHAR(50) NOT NULL,
    attachmentLink			VARCHAR(300) NOT NULL,
    user_id					INTEGER NOT NULL FOREIGN KEY ( user_id ) REFERENCES UserList ( id ) ON DELETE CASCADE,
    task_id					INTEGER NOT NULL FOREIGN KEY ( task_id ) REFERENCES task (id ) ON DELETE CASCADE
);



CREATE TABLE commentUser (
    id           INTEGER PRIMARY KEY IDENTITY,
    datecomment  DATE NOT NULL,
    commentUser  VARCHAR(300) NOT NULL,
    user_id      INTEGER NOT NULL FOREIGN KEY ( user_id ) REFERENCES UserList ( id ) ON DELETE CASCADE,
    task_id      INTEGER NOT NULL FOREIGN KEY ( task_id ) REFERENCES task ( id ) ON DELETE CASCADE
);

CREATE TABLE memberlist (
    user_id  INTEGER NOT NULL FOREIGN KEY ( user_id ) REFERENCES UserList ( id ) ON DELETE CASCADE,
    task_id  INTEGER NOT NULL FOREIGN KEY ( task_id ) REFERENCES task ( id) ON DELETE CASCADE,
    PRIMARY KEY (user_id, task_id)
);


CREATE TABLE timetrack (
    id          INTEGER PRIMARY KEY IDENTITY,
    starttime   DATE NOT NULL,
    endtime     DATE NOT NULL,
    isfinished  CHAR(1) NOT NULL,
    user_id     INTEGER NOT NULL FOREIGN KEY ( user_id ) REFERENCES UserList ( id ) ON DELETE CASCADE,
    task_id     INTEGER NOT NULL FOREIGN KEY ( task_id ) REFERENCES task (id) ON DELETE CASCADE
);