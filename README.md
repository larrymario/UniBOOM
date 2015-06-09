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
4. Delete all large-Unitychan data in UnityChan (UnityChan/Models), which is not needed.
 
Unitychan is so CUTE

## Audio Material Source
* [The Endless Fall](http://dova-s.jp/bgm/play3013.html) by Unish, as Title BGM
* [トグルスイッチ](http://dova-s.jp/bgm/play3135.html) by かずち, as Stage BGM
* [キラリきらめく](http://dova-s.jp/bgm/play2807.html) by yuki, as Stage BGM
* [Delight](http://dova-s.jp/bgm/play3079.html) by 龍崎一, as Stage BGM
* [ELIMINATE_LOCKED](http://dova-s.jp/bgm/play2838.html) by ISAo., as Stage BGM
* [Escape](http://dova-s.jp/bgm/play2597.html) by スエノブ, as Stage BGM
* [READY!](http://dova-s.jp/bgm/play2906.html) by ASKi, as Stage BGM
* [Tomorrow](http://dova-s.jp/bgm/play1869.html) by ISAo., as Ending BGM
* Sound Effect from [魔王魂](http://maoudamashii.jokersounds.com/)