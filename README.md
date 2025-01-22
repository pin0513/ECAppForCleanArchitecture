提供C# 的Clean Architecture架構下的WebApplication(WebApi)方案模版

Features
1. 支援.NET 8.0 + EF.Core 8.0 up  
3. 採用Clean Architecture的方案佈局, 採 Domain <= (Api => Application <= Infrastructure)
4. MediatR + CQRS Pattern抽象化Application層的BusinessLogic的標準行為，改善商業邏輯的可測試性
5. 支援整合測試，可進行銜接數據庫的商業邏輯的測試，搭配資料庫Reset套件, 採用自製半自動化管理資料庫版本更自動更新的機制(v0.1)
6. 單元測試框架採用NUnit+Moq.Net

專案生成來由
https://paulfun.net/wordpress/?p=4242



#Clean Architecture 簡介
Clean Architecture 是由 Robert C. Martin（Uncle Bob） 提出的軟體架構原則，主要目標是 分離關心點（Separation of Concerns），讓系統具有：
	1.	高可測試性（Testability）
	2.	高可維護性（Maintainability）
	3.	低耦合（Low Coupling）
	4.	可擴展（Scalability）
