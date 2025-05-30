---
name: Feature request
about: Suggest an idea for this project
title: ''
labels: ''
assignees: ''

---

name: 💡 Feature Request
description: 새로운 기능을 제안할 때 사용하세요
title: "[Feature]: "
labels: [enhancement]
body:
  - type: input
    id: feature_title
    attributes:
      label: 기능 이름
      placeholder: "예: 포션 아이템 추가"

  - type: textarea
    id: description
    attributes:
      label: 설명
      description: 어떤 기능이고, 왜 필요한지 서술해주세요
    validations:
      required: true
