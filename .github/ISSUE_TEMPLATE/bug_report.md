---
name: Bug report
about: Create a report to help us improve
title: ''
labels: ''
assignees: ''

---

name: 🐞 Bug Report
description: 버그를 제보할 때 사용하세요
title: "[Bug]: "
labels: [bug]
body:
  - type: markdown
    attributes:
      value: |
        감사합니다. 아래 항목을 최대한 상세히 작성해주세요.

  - type: input
    id: summary
    attributes:
      label: 문제 요약
      placeholder: "예: 게임 시작 시 캐릭터가 공중에 떠 있습니다"
    validations:
      required: true

  - type: textarea
    id: steps
    attributes:
      label: 재현 방법
      description: 재현 절차를 순서대로 적어주세요
    validations:
      required: true

  - type: textarea
    id: expected
    attributes:
      label: 기대 결과
    validations:
      required: false

  - type: textarea
    id: screenshot
    attributes:
      label: 스크린샷 또는 로그
    validations:
      required: false
