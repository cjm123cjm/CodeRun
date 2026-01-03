<template>
  <textarea ref="textAreaRef" class="editor"></textarea>
</template>

<script setup>
import { computed, nextTick, ref, getCurrentInstance, watch } from 'vue'
import suneditor from 'suneditor'
import 'suneditor/dist/css/suneditor.min.css'
import plugins from 'suneditor/src/plugins'
import lang from 'suneditor/src/lang/zh_cn'

const { proxy } = getCurrentInstance()

const props = defineProps({
  modelValue: {
    type: String,
    default: '',
  },
  extHeight: {
    type: Number,
    default: 380,
  },
  height: {
    type: Number,
  },
})

const height = computed(() => {
  if (props.height) {
    return props.height
  }

  return window.innerHeight - props.extHeight
})

const emit = defineEmits(['update:modelValue'])
const textAreaRef = ref()
const sunEditor = ref()

const init = () => {
  nextTick(() => {
    const editor = suneditor.create(textAreaRef.value, {
      lang: lang.zh_cn,
      height: height.value,
      plugins: plugins,
      buttonList: [
        ['undo', 'redo'],
        ['font', 'fontSize', 'formatBlock'],
        ['paragraphStyle', 'blockquote'],
        ['bold', 'underline', 'italic', 'strike', 'subscript', 'superscript'],
        ['fontColor', 'hiliteColor', 'textStyle'],
        ['removeFormat'],
        '/', // Line break
        ['outdent', 'indent'],
        ['align', 'horizontalRule', 'list', 'lineHeight'],
        ['table', 'link', 'image', 'video', 'audio' /** ,'math' */], // You must add the 'katex' library at options to use the 'math' plugin.
        /** ['imageGallery'] */ // You must add the "imageGalleryUrl".
        ['fullScreen', 'showBlocks', 'codeView'],
        ['preview', 'print'],
        ['save', 'template'],
        /** ['dir', 'dir_ltr', 'dir_rtl'] */ // "dir": Toggle text direction, "dir_ltr": Right to Left, "dir_rtl": Left to Right
      ],
      // 添加上传相关配置
      imageUploadHeader: {},
      imageUploadSizeLimit: 1024 * 1024 * 5, // 5MB
      imageUploadUrl: '', // 留空，使用自定义处理
    })

    editor.onImageUploadBefore = async (files, info, core, uploadHandler) => {
      // 验证文件
      if (!files || files.length === 0) {
        console.error('没有选择文件')
        return false
      }

      let result = await proxy.Request({
        url: '/Upload/UploadFile',
        method: 'post',
        params: {
          formFile: files[0],
        },
      })
      if (!result) return

      uploadHandler({
        result: [
          {
            url: proxy.globalInfo.imageUrl + res.result.url,
            name: files[0].name,
            size: files[0].size,
          },
        ],
      })

      return true
    }

    editor.onBlur = (e, core, content) => {
      emit('update:modelValue', content.innerHTML)
    }

    sunEditor.value = editor
  })
}

init()

watch(
  () => props.modelValue,
  (newVal) => {
    if (sunEditor.value) {
      sunEditor.value.setContents(newVal || '')
    }
  },
  { immediate: true, deep: true },
)
</script>

<style lang="scss" scoped>
.editor {
  width: 100%;
}
</style>
