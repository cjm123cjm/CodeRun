const PermissionCodes = {
  home: 'home',
  menu: {
    edit: 'settings_menu_edit',
  },
  role: {
    edit: 'settings_role_edit',
    del: 'settings_role_del',
  },
  account: {
    edit: 'settings_account_edit',
    del: 'settings_account_del',
    updatePwd: 'settings_account_update_password',
    updateStatus: 'settings_account_op_status',
  },
  category: {
    edit: 'category_edit',
    del: 'category_del',
  },
  question: {
    edit: 'question_edit',
    import: 'question_import',
    post: 'question_post',
    del: 'question_del',
    batchDel: 'question_del_batch',
  },
  exam: {
    edit: 'exam_question_edit',
    import: 'exam_question_import',
    post: 'exam_question_post',
    del: 'exam_question_del',
    batchDel: 'exam_question_del_batch',
  },
  share: {
    edit: 'share_edit',
    post: 'share_post',
    del: 'share_del',
    batchDel: 'share_del_batch',
  },
  app: {
    edit: 'app_update_edit',
    post: 'app_update_post',
  },
  carousel: {
    edit: 'app_carousel_edit',
  },
  feedback: {
    replay: 'app_feedback_replay',
  },
  appUser: {
    edit: 'app_user_edit',
  },
}

export default PermissionCodes
