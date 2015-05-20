# UniBOOM
This is a Unity-chan-starring Bomberman game.

## Third-Party Package - Unitychan
This project uses 4 third-party packages:
* SDユニティちゃん 3Dモデルデータ
* ユニティちゃんスクリプト（Unity 5 修正パッチ）
* ユニティちゃんシェーダー（修正パッチ）
* ユニティちゃん 3Dモデルデータ

Which could be downloaded here:

http://unity-chan.com/contents/guideline/

## Importing packages
1. Import the 4 package above.
2. You may find some alerts above duplicate shaders. Replace the shaders in SDUnityChan/Models with UnityChan/Models.
3. The materials of SDUnitychan (SDUnityChan/SD_Unitychan/Models/Materials) may be corrupted. If that happen, set the shaders of materials like below:
* def_mat -- UnityChan/Clothing - Double-sided
* hat_mat -- UnityChan/Hair - Double-sided
* mouth_mat -- UnityChan/Eye - Transparent
* nol_mat -- Hair - Double-side
* skin-mat -- Skin - Transparent
