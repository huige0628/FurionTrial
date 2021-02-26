<template>
    <el-dialog
        :visible.sync="show"
        :close-on-click-modal="false"
        :title="title"
        width="30%"
        top="10vh"
        scrollable
    >
        <div style="max-height: 550px; overflow-x: auto;" class="form-container">
            <el-form
                ref="form"
                :model="formData"
                :rules="formRules"
                label-width="100px"
            >
                <el-form-item label="模块名称" prop="moduleName">
                    <el-input
                        v-model="formData.moduleName"
                        placeholder="请输入角色名称"
                        clearable
                    />
                </el-form-item>
                <el-form-item label="模块标识" prop="code">
                    <el-input
                        v-model="formData.code"
                        placeholder="请输入模块标识"
                        clearable
                    />
                </el-form-item>
                <el-form-item label="排序" prop="sortNo">
                    <el-input
                        v-model="formData.sortNo"
                        placeholder="请输入排序"
                        clearable
                    />
                </el-form-item>
                <el-form-item label="图标" prop="icon">
                    <el-input
                        v-model="formData.icon"
                        placeholder="请输入角色名称"
                        clearable
                    />
                </el-form-item>
                <el-form-item label="Url" prop="url">
                    <el-input
                        v-model="formData.url"
                        placeholder="请输入角色名称"
                        clearable
                    />
                </el-form-item>
                <el-form-item label="备注" prop="remark">
                    <el-input
                        v-model="formData.remark"
                        :autosize="{ minRows: 5, maxRows: 8 }"
                        type="textarea"
                        placeholder="请输入备注"
                    />
                </el-form-item>
            </el-form>
        </div>
        <div slot="footer" class="dialog-footer">
            <el-button @click="close">取 消</el-button>
            <el-button type="primary" @click="ok">确 定</el-button>
        </div>
    </el-dialog>
</template>
<script>
/**
 * 用户添加编辑
 */
export default {
    name: 'RoleAddEdit',
    data() {
        return {
            show: false,
            loading: false,
            // 表单数据
            formData: {
                moduleId: null,
                moduleName: null,
                code: null,
                icon: null,
                url: null,
                sortNo: null,
                parentModuleId: null,
                remark: null
            },
            formRules: {
                moduleName: [
                    { required: true, message: '请输入模块名称', trigger: 'blur' }
                ],
                code: [{ required: true, message: '请输入模块标识', trigger: 'blur' }]
            }
        }
    },
    computed: {
        title() {
            return this.formData.userId ? '编辑' : '添加'
        }
    },
    mounted() {},
    methods: {
        // 确定
        ok: function() {
            this.$refs.form.validate(valid => {
                if (valid) {
                    this.loading = true
                    this.$api
                        .post('api/system/roleaddedit', this.formData)
                        .then(res => {
                            if (res.succeeded) {
                                this.$message.success('保存成功')
                                this.$emit('on-ok') // 成功响应事件
                                this.close()
                            } else {
                                this.$message.error(res.errors)
                            }
                        })
                        .catch(() => (this.loading = false))
                }
            })
        },
        // 关闭窗口
        close: function() {
            this.loading = false
            this.show = false
        },
        // 打开窗口
        open: function(moduleId) {
            this.show = true
            this.$nextTick(() => {
                this.$refs.form.resetFields()
                this.formData.moduleId = null
                if (moduleId) {
                    this.$api
                        .get('api/system/getmodulebyid', { params: { moduleId: moduleId } })
                        .then(res => {
                            if (res.data) {
                                this.form.moduleId = moduleId
                                this.form.moduleName = res.data.moduleName
                                this.form.code = res.data.code
                                this.form.icon = res.data.icon
                                this.form.url = res.data.url
                                this.form.sortNo = res.data.sortNo
                            }
                        })
                }
            })
        }
    }
}
</script>
<style>
.form-editor {
    padding-right: 5px;
}
</style>
