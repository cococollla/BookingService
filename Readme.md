# Booking service backend

## Запуск

**Через задачи:**
- 0:🐋👍 CONTAINER :: UP *- запускает только БД и SEQ*
- 1:🐋👍 CONTAINER :: UP FULL *- запускает БД, SEQ и само приложение*
- 2:🐋👍 CONTAINER :: FULL UP WITH BUILD *- полный запуск с предварительной сборкой*

**Через Powershell:**
```powershell

# Запускает только БД и SEQ
docker compose up -d

# Полный запуск с предварительной сборкой
docker compose --profile full up -d --build

```

## Логи

SEQ запускается на http://localhost:8088/

***SEQ бесплатный поддерживает только одного активного пользователя, так что закрываем вкладки после использования.***

**CorrelationId**
- копируем значение из headers ответа `X-Correlation-Id`
- в поиске SEQ указываем условие `CorrelationId = 'b5a9d269-bda9-408d-bf44-52f50e9e12f3'`

---
### Есть готовые таски (tasks.json и для rider)

Можно запускать через Terminal -> Run Task или F1 -> Run Task.

Но есть и очень удобные инструменты, можно оба ставить.
- https://marketplace.visualstudio.com/items?itemName=cnshenj.vscode-task-manager
- https://marketplace.visualstudio.com/items?itemName=spmeesseman.vscode-taskexplorer

## Структура решения

### Корневой уровень
```
BookingService/
├── _common/          # Общие компоненты
├── _docker/          # Docker конфигурации
│   └── compose.yaml  # Основной docker-compose файл
├── NewDomain/        # Компоненты конкретной сущности
└── Root.api/         # Корневой API проект
```

## Правила именования проектов

### 1. Префиксы для папок
- `_common` - общие компоненты (с underscore для приоритетного отображения)
- `_docker` - инфраструктурные файлы
- Без префикса - функциональные домены

### 2. Схема именования проектов внутри домена
```
{Domain}.{Layer}[.{Specific}]
```

**Пример для нового домена:**
```
NewDomain.api          # API слой
NewDomain.app          # Прикладной слой
NewDomain.bridge       # Модели, которые используются во всех 3х слоях(апи, бизнес, бд)
NewDomain.dal          # Data Access Layer
NewDomain.domain       # Доменная модель
NewDomain.job          # Фоновые задачи
NewDomain.shared       # Общие компоненты
NewDomain.shared.impl  # Реализации общих компонент
```

### 3. Слои архитектуры
- **`.api`** - Web API, контроллеры
- **`.app`** - Use Cases, Application Services
- **`.domain`** - Domain Models, Business Logic
- **`.dal`** - Data Access, Repository implementations
- **`.shared`** - Shared Kernel, Common abstractions
- **`.job`** - Background Jobs, Workers
- **`.bridge`** - Shared models