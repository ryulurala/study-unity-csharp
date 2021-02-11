---
title: "Unity 기초"
category: Unity-Framework
tags: [unity, visual studio code]
date: "2021-02-11"
---

## Unity 기초

### 환경 설정(`Unity`+`VSCode`)

- 그래픽 환경

1. Unity Hub 다운로드 & 설치
2. Unity 설치: `LTS`(Long Term Support) 버전(현재는 `2019.4.20f1`)
3. Unity Project 생성: Template-`3D`

- 스크립트 환경

1. Visual Studio Code 설치
2. VSCode Extension 설치
   - |                     C#                      |                           Debugger for Unity                            |                        Unity Tools                        |                            Unity Code Snippets                            |
     | :-----------------------------------------: | :---------------------------------------------------------------------: | :-------------------------------------------------------: | :-----------------------------------------------------------------------: |
     | ![C#](../uploads/unity-outline/csharp.jpeg) | ![Debugger for Unity](../uploads/unity-outline/debugger-for-unity.jpeg) | ![Unity Tools](../uploads/unity-outline/unity-tools.jpeg) | ![Unity Code Snippets](../uploads/unity-outline/unity-code-snippets.jpeg) |
3. Unity Code Editor 설정
   - | `Preferences`-`External Tools`-`External Script Editor`-`Visual Studio Code` |
     | :--------------------------------------------------------------------------: |
     | ![VSCode Unity Settings](../uploads/unity-outline/VSCode_unity-settings.png) |

### 화면 창(Window)

|   view    |               description                |
| :-------: | :--------------------------------------: |
|   Scene   | (=영화 세트장), GameObject가 배치된 View |
| Hierarchy |       GameObject의 List(Tree 형태)       |
|  Project  |     Project와 관련된 모든 파일 표시      |
|   Game    |   Main Camera에 의해 렌더링 되는 View    |
| Inspector |  선택한 GameObject에 대한 컴포넌트 정보  |
|    ...    |                   ...                    |

### 유용한 단축키

|                       단축키                       |                 description                 |
| :------------------------------------------------: | :-----------------------------------------: |
|                     `Ctrl`+`P`                     |                 Play / Stop                 |
|                 `Ctrl`+`Shift`+`N`                 |               New GameObject                |
|           `RMB Click`+`W`, `A`, `S`, `D`           |              Scene View 움직임              |
|             `RMB Click`+`Mouse Wheel`              |         Scene View 움직임 속도 조절         |
|                     `Ctrl`+`Z`                     |                  되돌리기                   |
|                    `W` in Scene                    |          Move tool(Object Control)          |
|                    `E` in Scene                    |                 Rotate tool                 |
|                    `R` in Scene                    |                 Scale tool                  |
| `Camera GameObject Click` + `Ctrl` + `Shift` + `F` | Scene View와 Camera View의 Transfrom 동기화 |
|               `Ctrl` + `Shift` + `C`               |            Console 창 뜨게 하기             |
|                        ...                         |                     ...                     |
