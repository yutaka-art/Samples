# Base64 エンコード・デコード 変換ツール

## 概要
このツールは、ドラッグ＆ドロップで指定されたファイルのテキストをBase64形式でエンコード・デコードすることができます。

## バージョン
1.0

## 動作環境
- 必須: .NET Core 7.0以上

## インストール方法
1. [リリースページ](https://github.com/yutaka-art/Samples/raw/main/Base64Converter/releases/Base64Converter.zip) から最新版をダウンロード。
2. ダウンロードしたZIPファイルを適当な場所に解凍。

## 使用方法
![説明](/Base64Converter/Images/00.png) 

1. `Base64Converter.exe`をダブルクリックで起動。
2. エンコード・デコードを選択するためのラジオボタンをチェック。※初期設定はエンコードです。
3. エンコードまたはデコードしたいファイルをアプリケーションにドラッグ＆ドロップ。
4. Base64Converterフォルダ配下、と、選択した.jsonフォルダの配下へ変換した結果が出力されます。
5. 終了する場合は、ウィンドウの×ボタンをクリック。

## 出力方式
### エンコード
``` :json
{
  "PostalCode" : "2700163"
}
```
→
``` :json
{
  "inputParam_Base64": "ew0KICAiUG9zdGFsQ29kZSIgOiAiMjcwMDE2MyINCn0="
}
```

### デコード
ファイル名:%元ファイル名%_Base64decode.json
``` :json
{
  "inputParam_Base64": "ew0KICAiUG9zdGFsQ29kZSIgOiAiMjcwMDE2MyINCn0="
}
```
→
``` :json
{
  "PostalCode" : "2700163"
}
```

## サポート・お問い合わせ
- メール: union.dml@gmail.com
- Issue: [GitHub Issues](https://github.com/yutaka-art/Samples/issues)

## 更新履歴
- v1.0 (2023-09-18): 初版リリース

## 注意
- 本ツールは、指定された用途のためだけに設計されています。適切な利用を心掛けてください。

## 関連リソース
- [GitHubリポジトリ](https://github.com/yutaka-art/Samples/)
