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

```

## 配布物に含めるもの
- SideBarBrowse.exe
- 同梱DLL/ランタイム一式
  - .NET ランタイム／Windows App SDK（Self-contained ビルドで生成されたファイル群）
- MicrosoftEdgeWebView2Setup.exe
  - WebView2 Evergreen ブートストラッパー（WebView2 ランタイム未導入環境向け）
- README.md
- LICENSE

## 主な機能詳細
- 固定位置とサイズ
  - 画面の左端または右端に縦いっぱい・横は所属モニタ幅の 25%（最小 200px）で固定
  - AppBar 機構で予約するため、他アプリを最大化した際は本アプリの領域を自動回避
- 位置選択
  - 初回起動時に「左端固定／右端固定」を必ず選択（保存され、次回から自動適用）
  - ツールバーのボタンからいつでも左右を切り替え可能
- アドレス欄の挙動
  - http:// または https:// で始まる文字列 → そのURLへ遷移
  - それ以外の入力 → Google 検索（https://www.google.com/search?q=...）
- 初期ページ
  - Google を表示
- ツールバー
  - 常時表示（戻る／進む／更新／アドレス欄／移動／左端固定／右端固定）

## 既知の挙動 / 制限
- AppBar の性質上、「最大化ウィンドウが領域を避ける」挙動です。通常サイズのウィンドウは重ねることができます
- マルチモニタでモニタを跨いで移動した場合は、左右固定ボタンで再固定すると当該モニタ幅で再計算されます
- 高DPI環境でも概ね問題ありませんが、環境によっては表示が変わることがあります

## セキュリティとプライバシー
- 本アプリ自体は利用状況を収集しません
- Web表示は WebView2（Microsoft Edge）に準拠します。各ウェブサイトのポリシーに従います

## AI支援（ChatGPT）の利用について
本リポジトリの設計、コード断片、ドキュメント（本READMEを含む）の作成・改善に ChatGPT を多用しました。最終的な統合・検証は人手で行っていますが、誤りが残る可能性があります。問題を見つけた場合は Issue や PR でお知らせください。  
AIで生成したテキストやコードが含まれる箇所は、プロジェクトのライセンス（MIT）に従います。第三者コンポーネント（WebView2 Runtime など）は各ライセンスに従います。

## ライセンス
- 本リポジトリのコードとドキュメントは MIT License（LICENSE）で提供します
- バンドル/依存コンポーネント（.NET Runtime, Windows App SDK, WebView2 Runtime など）は各プロジェクトのライセンスに従います

## 謝辞
- Microsoft .NET MAUI
- Windows App SDK
- Microsoft Edge WebView2
- Microsoft Docs およびコミュニティの皆さま
- OpenAI ChatGPT（設計・コード/ドキュメント作成の支援に活用）

