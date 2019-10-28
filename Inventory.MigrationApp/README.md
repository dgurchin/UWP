Перед началом работы:
1) Начальным проектом компиляции выбрать <b>Inventory.MigrationApp</b>
2) Начальным проектом в <b>Package Manager Console</b> выбрать <b>Inventory.Data</b>

Команда для создания начальной миграции<br/>
<b>Add-Migration InitialCreate -StartupProject Inventory.MigrationApp -Project Inventory.Data -Context SQLServerDb</b>

Команда для удаления последней миграции<br/>
<b>Remove-Migration -Context SQLServerDb</b>
