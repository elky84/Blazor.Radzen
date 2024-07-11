# MinimalApi

C# ASP.NET Minimal API를 이용하는 예시

## Getting Started

### MinimalApi/AppSettings.json

ConnectionStrings안에 환경에 맞는 값을 입력해주세요

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Debug"
    }
  },
  "ConnectionStrings": {
    "DB": "Server=localhost; Port=3306; Database=MinimalApi; Uid=root; Pwd=localhost; Pooling=true; default command timeout=120;MinimumPoolSize=2;"
  }
}
```

### mysql script

```sql
CREATE DATABASE `MinimalApi`;
```

```sql
DROP TABLE IF EXISTS Account;
CREATE TABLE Account
(
    AccountUid BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
    AccountId VARCHAR(36) NOT NULL DEFAULT '' COMMENT '계정ID',
    AccountPassword VARCHAR(36) NOT NULL DEFAULT '' COMMENT '계정암호',
    AccountStatus TINYINT NOT NULL DEFAULT '0' COMMENT '계정상태',
    Detail JSON NOT NULL COMMENT 'JSON',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '수정일시',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성일시',
    PRIMARY KEY (AccountUid),
    UNIQUE UixAccountId (AccountId)
)

ENGINE = InnoDB
DEFAULT CHARSET = utf8mb4
COLLATE = utf8mb4_unicode_ci;
```