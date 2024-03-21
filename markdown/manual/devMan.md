# 環境構築手順書

# 1 本書について

本書では、3D都市モデルに最適化したVPS（以下「本システム」という。）の利用環境構築手順について記載しています。本システムの構成や仕様の詳細については以下も参考にしてください。

[技術検証レポート](https://xxx)

# 2 動作環境

本システムの動作環境は以下のとおりです。




| 項目 | 推奨動作環境 | 
| - | - |
| Unity Version | Unity 2021.3.27f1 LTS | 
| AR Framework | AR Foundation 4.2.8 <br> ARKit XR Plugin 4.2.9 <br> Pretia SDK 0.11.0 | 
| Supported Unity Editor Platforms | iOS　(Minimum iOS Version: iOS 12) | 
| デバイス | iPhone X　以降 | 
| Xcode Version |　Xcode 15.0.1 | 


# 3 インストール手順

### Unityの設定
* プロジェクト設定の更新
  * Unityで、File > Build Settingsを選択してください。
  * プラットフォームとしてiOSを選択します。まだインストールされていない場合は、Add Moduleをクリックし、指示に従ってiOSサポートをインストールしてください。
  * Player Settingsをクリックし、Settings for iOSタブで設定をカスタマイズします：
    * Identification（通常、com.yourcompany.yourappのような逆ドメイン名形式）でBundle Identifierを設定します。
    * Build Settingsウィンドウを閉じます。

# 4 ビルド手順

* Xcode Projectの作成
  * Unity Editorに戻り、Build SettingsウィンドウでBuild And Runをクリックします。Unityがプロジェクトをコンパイルし、Xcodeプロジェクトを作成します。
  * Xcodeプロジェクトを保存する場所を選択し、[Save]をクリックします。
  * ビルドが完了すると、UnityはXcodeプロジェクトを開きます。開かない場合は、保存した場所に移動し、.xcodeprojファイルを開いてください。
* iOS用のビルド
  * XcodeのProject Navigatorでプロジェクトを選択します。正しいターゲットが選択されていることを確認してください。
  * Apple Developerアカウントでサインインします。Xcode > Preferences > Accounts に進み、アカウントを追加します。
  * Signing & Capabilitiesで正しいチームを選択します。
  * iOSデバイスをUSB経由でMacに接続します。
  * Build and Runボタンをクリックします。Xcodeがデバイスにアプリをインストールします。

# 5 準備物一覧

特になし
