![image](https://github.com/Project-PLATEAU/PLATEAU-PretiaVPS-AR-app/assets/79615787/88a54201-e6bd-422a-92d3-f094698102f1)

# PLATEAU PretiaVPS ARアプリ

## 1. 概要
本リポジトリでは、2023年度にProject PLATEAUが開発した「3D都市モデルに最適化したVPS」（スマートフォン向け）の実証用アプリケーションを公開しています。
「3D都市モデルに最適化したVPS」（スマートフォン向け）は、PLATEAUの3D都市モデルをVPSマップとして利用する仕組みを構築することで、スケーラブルなVPSを実現し、自動運転やAR体験など高精度自己位置測位を必要とする様々なソリューションを実現していくことを目的に開発されています。


## 2.  「3D都市モデルに最適化したVPS」（スマートフォン向け）について
「3D都市モデルに最適化したVPS」（スマートフォン向け）は、PLATEAUの3D都市モデルから生成した点群マップと、スマートフォンで撮影したカメラ画像から生成した点群マップとを比較し、自己位置を推定するシステムを開発しました。  
本システムは、PLATEAUの3D都市モデル（LOD3）が整備された沼津の特定エリア(１０箇所)において、アプリケーションで撮影したカメラ画像から生成した点群マップと、3D都市モデルから生成した点群マップとを比較し、自己位置を推定する機能を実装しています。  

実証用アプリケーションでは、周辺の建物に関連するアイコン(3Dコンテンツ)をAR表示する機能、PLATEAUの3D都市モデルをAR表示する機能（透過度調整可）を実装しています。
実証用アプリケーションは、プレティア・テクノロジーズ株式会社が保有するAR開発プラットフォームPretiaをもとに、簡易的な自己位置推定の精度評価用スマートフォン向けARアプリケーションと合わせて開発されています。

本システムの詳細については
[技術検証レポート](https://www.mlit.go.jp/plateau/file/libraries/doc/plateau_tech_doc_0088_ver01.pdf)を参照してください。

- 留意事項
 - 本リポジトリで提供する実証用アプリケーションでは、VPSによる自己位置推定処理をプレティア・テクノロジーズ社が運用するPretiaサーバーにおいて行っています。
 - Pretiaサーバー及びその機能は予告なく変更または削除する可能性がございます。

  
## 3. 利用手順
本システムの構築手順及び利用手順については[利用チュートリアル](https://project-plateau.github.io/PLATEAU-PretiaVPS-AR-app/)を参照してください。

## 4.  システム概要
* **ARコンテンツの表示**: 
利用者が、本プロジェクトで指定する沼津市のチェックポイントにて、アプリ上からカメラを起動しスキャンを実行することで、アプリケーションで撮影したカメラ画像から生成した点群マップと3D都市モデルから生成した点群マップとを比較し、自己位置を推定。認識した空間上にARコンテンツを配置します。

* **オクルージョン**: "オクルージョン"機能によって、ARコンテンツは現実世界と同じように、ビルの後ろや中にあるように見えるリアルな効果を生み出します。
* **コンテンツ位置情報の表示** : この機能は、Plateau SDKから算出したARコンテンツの位置を表示します。ARコンテンツがグローバル座標のどこに配置されているかを表示します。
* **デバイス位置情報の表示** : 画面下部に、あなたのデバイスの座標を2種類表示します：
  * **GPS座標**: GPSで取得したデバイスの位置情報を表示します。
  * **相対座標**: VPSで取得した相対座標とPlateau SDKからデバイスのグローバル座標を算出し表示します。


## 5.  利用技術
|名称 |概要 |
|-|-|
| SfM　（Structure from motion） | 複数枚の画像から対象の形状を復元する技術。画像内の特徴点をもとにVPS用の点群を出力する。3D都市モデルからの点群出力アルゴリズムの要素技術として利用する。 |
| 点群to点群対応化アルゴリズム | - 異なる２つの点群データ（①現実世界から変換した点群データと②3D都市モデルから変換した点群データ）をもとに座標情報等を整理し双方の対応関係を導出し、同一の処理で扱える点群へ変換するアルゴリズム。 <br> <br> - Pretiaが既存でもつアルゴリズムをベースに公知の論文情報等を参照しながらモデル及びパラメータを調整し、Pretiaの点群出力アルゴリズムで抽出した①及び②の点群の対応に最適化する。 |
| 自己位置推定アルゴリズム（PLATEAU-to-Pretia VPS） | 実世界から出力した点群と、スマートフォン向けARアプリケーションから送られたカメラ映像から出力された点群をマッチングし、自己位置情報（x1, y1, z1）を返す処理をおこなう。本プロジェクトでは、Pretiaが所有するアルゴリズムをそのまま適用する。 |
| 座標変換アルゴリズム | PLATEAU-to-Pretia VPSにて出力した自己位置情報（x1, y1, z1）を点群to点群対応化アルゴリズムで出力したメタデータ（座標変換マトリクス）で3D都市モデルに最適化した自己位置情報（x2, y2, z2）へ変換する処理。本プロジェクトでは、Pretiaが所有するアルゴリズムをベースに一部調整を行う。 |


## 6.  動作環境

| 項目 | 推奨動作環境 | 
| - | - |
| Unityのバージョン | Unity 2021.3.27f1 LTS | 
| ARフレームワーク | AR Foundation 4.2.8 <br> ARKit XR Plugin 4.2.9 <br> Pretia SDK 0.11.0 | 
| Unityエディタ対応OS | iOS　(Minimum iOS Version: iOS 12) | 
| デバイス | iPhone X　以降 | 
| Xcodeのバージョン |　Xcode 15.0.1 | 


## 7.  本リポジトリのフォルダ構成
| フォルダ名 |　詳細 |
|-|-|
| Assets | Scenes, PrefabsなどUnity用のAssetsデータを管理 |
| Packages | Unity用のPackageを管理 |
| ProjectSettings | Unity用のProjectSettingsデータを管理（Unityの[Edit] > [Project Settings]で編集された情報）|
| Docs/Images | Readme用の画像などを管理 |


## 8.  ライセンス

- ソースコードの著作権はプレティア・テクノロジーズ社に帰属、ドキュメントの著作権は国土交通省に帰属します。
- 本ドキュメントは[Project PLATEAUのサイトポリシー](https://www.mlit.go.jp/plateau/site-policy/)（CCBY4.0及び政府標準利用規約2.0）に従い提供されています。
- ソースコードのライセンス詳細は[LICENSE](LICENSE) に従い提供されています。

また、本システムでは、下記CC0による3Dコンテンツを利用しております。

- [Cake!!](https://alpha.womp.com/preview/387526) by [Movieinyou](https://alpha.womp.com/profile/f7394afc-6e94-40d4-8bc5-d9014f997e42)
- [Shrimp ramen](https://alpha.womp.com/preview/448890) by [Oleksandra](https://alpha.womp.com/profile/d4b238ac-388e-40ad-88de-05f86d7639f5)
- [CORNUCOPIA](https://alpha.womp.com/preview/91692) by [artlylee](https://alpha.womp.com/profile/6f769f65-bc71-4a9d-8a22-83fd55113874)
- [tooth](https://alpha.womp.com/preview/457979) by [Rulfo](https://alpha.womp.com/profile/4d0c1893-dcbc-4e38-b113-72015c232c7b)
- [China's Food](https://alpha.womp.com/preview/108889) by [Marta Cvetkova](https://alpha.womp.com/profile/c901d31f-adca-4b82-b9c5-575bccd518cf)
- [Sugar cookies](https://alpha.womp.com/preview/92650) by [Speedy](https://alpha.womp.com/profile/97490f3b-9324-477e-9a25-822960196835)
- [Pre-Latte](https://alpha.womp.com/preview/456671) by [JONUNO](https://alpha.womp.com/profile/c94d13a3-c1be-482b-90ab-fa2fe717f377)
- [Glass Vanity](https://alpha.womp.com/preview/157169) by [heizal](https://alpha.womp.com/profile/30808621-65d7-4d30-b63c-eb700d209e33)
- [CARTOON CONE](https://alpha.womp.com/preview/468005) by [Rulfo](https://alpha.womp.com/profile/4d0c1893-dcbc-4e38-b113-72015c232c7b)
- [Golb](https://alpha.womp.com/preview/393543) by [Mr](https://alpha.womp.com/profile/a0d99e2e-e8d1-4706-bed3-37ed842e40b3)
- [Good Tv for sale](https://alpha.womp.com/preview/361870) by [NoodlesCan](https://alpha.womp.com/profile/13d30dc9-9a14-4059-8857-f4d76cd35d56)
- [simple monopoly house](https://alpha.womp.com/profile/639b35af-85cf-4307-93c2-ecec4ffb997c) by [womp_for_3Dprinting](https://alpha.womp.com/profile/639b35af-85cf-4307-93c2-ecec4ffb997c)
- [talking tree](https://alpha.womp.com/preview/391682) by [Benjamin Browning](https://alpha.womp.com/profile/8610fe94-b7b9-4724-8870-3af2c1930da9)
- [Longbush Templa](https://alpha.womp.com/preview/29416) by [Alexandra Stobiecka](https://alpha.womp.com/profile/cabcc539-fefd-458d-a7cd-aa8f18710f6c)
- [CanScene attempt](https://alpha.womp.com/preview/35462) by [Eduardo Gutierréz](https://alpha.womp.com/profile/5c07458f-59aa-4db4-a380-208f27d5df38)
- [Oligastiri](https://alpha.womp.com/preview/46326) by [ssen](https://alpha.womp.com/profile/9cc1fa4b-c647-484e-b74f-ad6709c97429)
- [Bottle 02 v2](https://alpha.womp.com/preview/423803) by [Maxwell Willman](https://alpha.womp.com/profile/d679d2fe-af3f-475b-b0b8-15547e0b0209)

## 9.  注意事項

- 本リポジトリは参考資料として提供しているものです。動作保証は行っていません。
- 本リポジトリについては予告なく変更又は削除をする可能性があります。
- 本リポジトリの利用により生じた損失及び損害等について、国土交通省はいかなる責任も負わないものとします。


### オープンソースへのコントリビュートについて

オープンソースへのコントリビュートに興味をお持ちいただきありがとうございます。以下にコントリビュートに向けた簡易手順を記載します。:

* 環境がプロジェクトの要件を満たしていることを確認します。リポジトリのクローンを作成します。
* 未解決の課題を選択するか、課題を新規作成して変更を提案してください。
* ローカル・リポジトリに、わかりやすい名前で新しいブランチを作成してください。
* プロジェクトのコーディング標準に従って、変更を実装してください。
* ブランチをGitHubにプッシュし、プルリクエストを作成してください。
* 承認されると、管理者があなたの変更を取り込みます。


## 10.  参考資料

- 技術検証レポート: https://www.mlit.go.jp/plateau/file/libraries/doc/plateau_tech_doc_0088_ver01.pdf
- PLATEAU WebサイトのUse caseページ「3D都市モデルに最適化したVPSの開発等」: https://www.mlit.go.jp/plateau/use-case/uc23-18/

