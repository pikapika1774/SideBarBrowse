# SideBarBrowse

Windows の画面左右に「縦いっぱい・横1/4幅」で常駐できるサイドバー型のミニブラウザです。AppBar（タスクバーと同じ仕組み）を利用しているため、他アプリを最大化したときは本アプリの領域を自動で避けます。

- 初期ページは Google
- アドレス欄が http(s) で始まらないときは Google 検索にフォールバック
- ツールバーは常時表示（戻る/進む/更新/アドレス/移動＋左端固定/右端固定）
- 初回起動時は固定位置（左/右）を必ず選択。以後はツールバーからいつでも切り替え可能
- マルチモニタ対応（所属モニタ幅の 1/4 を確保）
- バナー（アプリ名タイトル）非表示

## 動作環境
- Windows 10 2004 (build 19041) 以降、または Windows 11
- WebView2 Runtime（未導入の場合は同梱の MicrosoftEdgeWebView2Setup.exe でインストール可能、管理者権限不要）

## 使い方（配布物から実行する場合）
1. リリースの ZIP を任意のフォルダに展開
2. WebView2 Runtime が無い場合は `MicrosoftEdgeWebView2Setup.exe` を実行
3. `SideBarBrowse.exe` を起動
4. 初回のみ「左端固定 / 右端固定」を選択（設定は保存されます）
5. ツールバーの「左端固定 / 右端固定」でいつでも切り替え可能

スタートアップ登録（自動起動）:
- Win+R → `shell:startup` → このフォルダに `SideBarBrowse.exe` のショートカットを置く

## ビルド（開発者向け）
前提
- .NET 9 SDK
- Visual Studio 2022（.NET MAUI workload インストール済み）または `dotnet` CLI
- Windows 10/11

手順（CLI）
```bash
# 依存の復元
dotnet restore .\SideBarBrowse\SideBarBrowse.csproj

# デバッグ実行
dotnet build .\SideBarBrowse\SideBarBrowse.csproj -c Debug
dotnet run --project .\SideBarBrowse\SideBarBrowse.csproj -c Debug


