# Avatar Bone Sync for MA

VRChatアバター向けのボーン調整支援ツールです。  
Bone utility tool for VRChat avatars.  

[日本語](#日本語) | [English](#english)

---

## 日本語

### 概要
**Avatar Bone Sync for MA** は、VRChatアバターのボーン構造間で  
以下の要素を安全にコピーするための Editor 拡張ツールです。

- Transform（Position / Rotation / Scale）
- ModularAvatar ScaleAdjuster

---

### 主な機能

- シンプルモード / カスタムモード
- Transform のコピー（位置・回転・スケール）
- ModularAvatar ScaleAdjuster のコピー
- Hierarchy 右クリックから即実行（Execution）
- VRC Constraint 系コンポーネントは自動除外

---

### 動作環境

- Unity 2022.3 以上
- VRChat Creator Companion
- Modular Avatar

---

### インストール方法（VCC）

### VCC ワンクリック追加

VRChat Creator Companion に Avatar Bone Sync for MA のリポジトリを直接追加できます。  
[VCCに追加](vcc://vpm/addRepo?url=https://zbelqcraft.github.io/AvatarBoneSync/vpm.json)

---

### 使い方（基本）

#### Execution版（おすすめ）
1. Hierarchy 上の任意のオブジェクトを右クリック
2. **Modular Avatar → Setup Outfit**
3. 同じオブジェクトを再度右クリック
4. **Avatar Bone Sync for MA → Execution**
5. 完了

#### Tool版
1. Unity メニューから  
   **Tools → Avatar Bone Sync for MA**
2. コピー元ボーンとコピー先ボーンを指定
3. 必要な設定を選択して実行

##### Tool版 使用例  
- HeadからHead(または近似ボーン)にコピーしたい場合

---

### 注意事項

- VRCRotationConstraint / VRCConstraint 系はコピー対象外です
- 本ツールは Editor 専用です（Runtime では動作しません）

---

## English

### Overview
**Avatar Bone Sync for MA** is an Editor utility tool for VRChat avatars.  
It safely copies the following elements between bone hierarchies:

- Transform (Position / Rotation / Scale)
- ModularAvatar ScaleAdjuster

The tool is designed with simplicity and safety in mind.

---

### Features

- Simple mode / Custom mode
- Transform copy (Position, Rotation, Scale)
- ModularAvatar ScaleAdjuster copy
- Hierarchy right-click execution
- Automatically excludes VRC Constraint components

---

### Requirements

- Unity 2022.3 or later
- VRChat Creator Companion
- Modular Avatar

---

### Installation (VCC)

### One-click VCC Install

Click the link below to add the Avatar Bone Sync for MA repository directly to VRChat Creator Companion.  
[Add_to_VCC](vcc://vpm/addRepo?url=https://zbelqcraft.github.io/AvatarBoneSync/vpm.json)


---

### Usage

#### Execution Version (Recommended)
1. Right-click any object in the Hierarchy
2. Select **Modular Avatar → Setup Outfit**
3. Right-click the same object again
4. Select **Avatar Bone Sync for MA → Execution**
5. Done

#### Tool Version
1. Open **Tools → Avatar Bone Sync for MA**
2. Assign source and target Transforms
3. Configure settings and execute

#### Tool Version Example
- Copy transforms from **head** to **head(or nearly transform)**

The Execution Version is recommended for most users, as it works automatically with Modular Avatar outfits.


---

### Notes

- VRCRotationConstraint and VRCConstraint components are excluded
- Editor-only tool (not included in runtime builds)

---

### License
MIT License

