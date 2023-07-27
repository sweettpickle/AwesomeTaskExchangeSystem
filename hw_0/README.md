Верхнеуровневое описание системы:
-
![image](https://github.com/sweettpickle/AwesomeTaskExchangeSystem/assets/44313432/e265b238-4baf-45f9-918c-257d438ca26a)


ParrotManagement
-
- Отвечает за создание/удаление/изменение новых попугов, выдачу им прав, хранит их роль и тд

**Описание API:**
- POST Parrot - создание попуга
- PUT Parrot - изменение данных попуга
- POST GrantPermission - выдача прав
- POST RevokePermission - отмена разрешения

**Схема БД:**
![image](https://github.com/sweettpickle/AwesomeTaskExchangeSystem/assets/44313432/12524f13-f009-4c08-899f-91d97cc246ff)


TaskManager
-
- Отвечает за создание/изменение задачи, за назначении задач на попугов

**Описание API:** 


**Схема БД:**
![image](https://github.com/sweettpickle/AwesomeTaskExchangeSystem/assets/44313432/b5e8e515-924f-43cd-889a-3d9e72eacdf7)
