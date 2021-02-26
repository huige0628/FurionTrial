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
                <el-form-item label="部门" prop="orgName">
                    <el-input
                        v-model="formData.orgName"
                        placeholder="请输入部门名称"
                        clearable
                    />
                </el-form-item>
                <el-form-item label="上级部门">
                    <TreeSelect
                        ref="orgTree"
                        v-model="formData.parentOrgId"
                        :options="orgTreeData"
                        :props="defaultProps"
                        placeholder="请选择上级部门"
                    />
                </el-form-item>
                <el-form-item label="状态" prop="status">
                    <el-radio-group v-model="formData.status">
                        <el-radio :label="1">启用</el-radio>
                        <el-radio :label="0">禁用</el-radio>
                    </el-radio-group>
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
    name: 'OrgAddEdit',
    data() {
        return {
            show: false,
            loading: false,
            // 表单数据
            formData: {
                orgId: null,
                orgName: null,
                parentOrgId: null,
                parentOrgName: null,
                status: 1
            },
            formRules: {
                orgName: [{ required: true, message: '请输入部门', trigger: 'blur' }]
            },
            orgTreeData: [],
            defaultProps: {
                parent: 'parentId', // 父级唯一标识
                value: 'id', // 唯一标识
                label: 'label', // 标签显示
                children: 'children' // 子级
            }
        }
    },
    computed: {
        title() {
            return this.formData.userId ? '编辑' : '添加'
        }
    },
    mounted() {
        this.loadOrgTree()
    },
    methods: {
        // 确定
        ok: function() {
            this.$refs.form.validate(valid => {
                if (valid) {
                    if (!this.formData.parentOrgId) {
                        this.$message.warning('请选择上级部门')
                        return
                    }
                    this.formData.parentOrgName = this.$refs.orgTree.queryTree(
                        this.orgTreeData,
                        this.formData.parentOrgId
                    )
                    this.loading = true
                    this.$api
                        .post('api/system/orgaddedit', this.formData)
                        .then(res => {
                            if (res.success) {
                                this.$message.success(res.data)
                                this.$emit('on-ok') // 成功响应事件
                                this.close()
                            } else {
                                this.$message.error(res.msg)
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
        open: function(row) {
            this.show = true
            this.$nextTick(() => {
                this.$refs.form.resetFields()
                this.formData.orgId = null
                this.formData.parentOrgId = null
                this.formData.parentOrgName = null
                if (row) {
                    this.formData.orgId = row.orgId
                    this.formData.orgName = row.orgName
                    this.formData.parentOrgId = row.parentOrgId.toString()
                    this.formData.parentOrgName = row.parentOrgName
                    this.formData.status = row.status
                }
            })
        },
        loadOrgTree() {
            this.$api
                .get('api/system/getorgtree', { params: { isRoot: true } })
                .then(res => {
                    this.orgTreeData = res.data
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
