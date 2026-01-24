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
👉 [VCC に追加](vcc://vpm/addRepo?url=https://zbelqcraft.github.io/AvatarBoneSync/vpm.json)


---

### 使い方（基本）

#### Execution版（おすすめ）
1. Hierarchy 上の任意のオブジェクトを右クリック
2. **Avatar Bone Sync for MA → Execution**
3. ModularAvatar MergeArmature を自動検出して実行

#### Tool版
1. Unity メニューから  
   **Tools → Avatar Bone Sync for MA**
2. コピー元ボーンとコピー先ボーンを指定
3. 必要な設定を選択して実行

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
👉 [Add to VCC](vcc://vpm/addRepo?url=https://zbelqcraft.github.io/AvatarBoneSync/vpm.json)


---

### Usage

#### Tool Version
1. Open **Tools → Avatar Bone Sync for MA**
2. Assign source and target Transforms
3. Configure settings and execute

#### Execution Version (Recommended)
1. Right-click an object in the Hierarchy
2. Select **Avatar Bone Sync for MA → Execution**
3. The tool automatically finds MergeArmature and executes

---

### Notes

- VRCRotationConstraint and VRCConstraint components are excluded
- Editor-only tool (not included in runtime builds)

---

### License
MIT License

