# ねるこ我女兒 Hi~ o(*￣▽￣*)ブ

[![Build Status](https://dev.azure.com/louder-zone/Neruko%20Project/_apis/build/status/louderzone.neruko-project?branchName=master)](https://dev.azure.com/louder-zone/Neruko%20Project/_build/latest?definitionId=1&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=lz.neruko&metric=alert_status)](https://sonarcloud.io/dashboard?id=lz.neruko)

一起來打造最強換班能手吧！！

## 新手入門

### [專案維基](https://github.com/louderzone/neruko-project/wiki)

如果你對參與編程感到興趣，可以先參閱[專案維基](https://github.com/louderzone/neruko-project/wiki)，來了解一下計劃需求。

### [新手教學](https://github.com/louderzone/neruko-project/issues?q=label%3A%E6%96%B0%E6%89%8B%E6%95%99%E5%AD%B8)

對維基內容，或者計劃細節有任何不理解的地方，可以試著先搜索一下標記著「[新手教學](https://github.com/louderzone/neruko-project/issues?q=label%3A%E6%96%B0%E6%89%8B%E6%95%99%E5%AD%B8)」的議題，當中可能已經有人問過同樣的問題，並得到開發者的解答。（提示：搜索功能可以對內文也有效，可以先試著從一些關鍵字搜索，解答方案不一定在標題上反映的）

如果還是解答不到你心中的疑惑，可以提交一個「[新議題](https://github.com/louderzone/neruko-project/issues/new/choose)」，我們會盡快回覆你的。

### [入何開始？](#解決方案堆疊)

如果你有少量編程經驗，可以試著先掌握本專案使用的[解決方案堆疊](#解決方案堆疊)。列出的堆疊都有附上官方教學的鏈接，試著先從征服各個Getting Started吧！

### Q&A

**問：我不懂編程，可以做什麼？**

> 提供意見也是開發的一部份哦~~ 你可以先到「[議題列表](https://github.com/louderzone/neruko-project/issues)」看一下現在正在進行的工序。「[新人向](https://github.com/louderzone/neruko-project/labels/%E6%96%B0%E4%BA%BA%E5%90%91)」的議題會是一個好開始。

**問：我是新手，也可以加入開發嗎？**

> 我們非常歡迎任何人參與開發！如果你對你的編程能力感到疑惑，可以先觀察一下現有代碼，相信很快就上手！
>
> 另外，本計劃利用各種 [持續整合(CI)](https://dev.azure.com/louder-zone/Neruko%20Project/_build/latest?definitionId=1&branchName=master) / [靜態程式分析](https://sonarcloud.io/dashboard?id=lz.neruko) 工具來保證開發的水準。如果水準不達標，可以試著根據建議改良，如此一來，也可以增強自己的能力，一舉兩得。

## 入門要求

### 環境

- Visual Studio 2019 Community 或以上 (VS Code也可以喇)
- .NET Core 3.1 SDK

### 解決方案堆疊

- [C#](https://channel9.msdn.com/Series/C-Fundamentals-for-Absolute-Beginners)
- [ASP.NET Core with Blazor](https://docs.microsoft.com/zh-tw/aspnet/core/tutorials/build-a-blazor-app?view=aspnetcore-3.1)
- [DSharpPlus](https://dsharpplus.emzi0767.com/articles/intro.html)

## 計劃藍本

**問：所以，講了這麼久，你女兒是幹什麼用的喇w？**

> 基本上就是遊戲輔助人形 x) 只要你想得到的遊戲，都可以本程序作為藍本加入。 此計劃重視多平台整合，提供各種推送服務，及各方數據收集。旨在以一個「人」的形式來融入大家。

目前此計劃大體分為三個部份：操作界面，大數據收集，人工智能

### 操作界面

前端的操作，提供一系列可以手動輸入數據的小工具。例如：班表安排工具

### 大數據收集

數據收集，在各社交平台，甚至第三方平台，收集各種遊戲資料。例如：玩家習慣，實時排名分析等。

### 人工智能

面向用家的ねるこ醬，以對答的方式提供各方協助， ~~包括參與幹話~~。目前以Discord為主。延伸方面考慮Line，Twitter，甚至單獨的AI程式。

### 整合

最後是本計劃的核心，就是以上三項的整合。透過整理大數據與手動輸入的資料，為玩家提供各種容易理解及存取的資料。

ねるこ將透過對話理解用家的需求，並提供各種貼心合適的協助。

例如但不限於：~~提醒跑者喝水~~，排名危機警報，還可以理解用家的問題，如「現在100位多少？」，馬上提供合適資訊。

> 或者計劃看起來毫不吸引，但確實涵括多個人工智能範疇  
> ~~其實我就只是想弄一個可以自然對答的機器人而已~~

## 開發團隊

- 負責人 - 冬雪桜 （[KokomiShiina](https://github.com/KokomiShiina)）
