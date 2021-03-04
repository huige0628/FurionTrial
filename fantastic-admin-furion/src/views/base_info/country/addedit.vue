<template>
    <el-dialog
        :visible.sync="show"
        :close-on-click-modal="false"
        :title="title"
        width="40%"
        top="10vh"
        scrollable
    >
        <div style="max-height: 550px; overflow-x: auto;" class="form-container">
            <el-form ref="form" :model="formData" :rules="formRules" label-width="100px">
                <el-row>
                    <el-col :span="12">
                        <el-form-item label="洲信息" prop="continent">
                            <el-select v-model="formData.continent" placeholder="请选择洲信息" style="width: 100%;">
                                <el-option label="亚洲" value="shanghai" />
                                <el-option label="欧洲" value="beijing" />
                            </el-select>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item label="国家二字码" prop="twoCode">
                            <el-input v-model="formData.twoCode" placeholder="请输入国家二字码" clearable />
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row>
                    <el-col :span="12">
                        <el-form-item label="简体中文名" prop="cnName">
                            <el-input v-model="formData.cnName" placeholder="请输入简体中文名" clearable />
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item label="繁体中文名" prop="twName">
                            <el-input v-model="formData.twName" placeholder="请输入繁体中文名" clearable />
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row>
                    <el-col :span="12">
                        <el-form-item label="英文名称" prop="enName">
                            <el-input v-model="formData.enName" placeholder="请输入英文名称" clearable />
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item label="国家三字码" prop="threeCode">
                            <el-input v-model="formData.threeCode" placeholder="请输入国家三字码" clearable />
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row>
                    <el-col :span="12">
                        <el-form-item label="其它简码" prop="otherCode">
                            <el-input v-model="formData.otherCode" placeholder="请输入其它简码" clearable />
                        </el-form-item>
                    </el-col>
                    <el-col :span="12" />
                </el-row>
                <el-row>
                    <el-form-item label="备注" prop="remark">
                        <el-input
                            v-model="formData.remark"
                            type="textarea"
                            placeholder="请输入备注"
                            maxlength="250"
                            show-word-limit
                            clearable
                        />
                    </el-form-item>
                </el-row>
            </el-form>
        </div>
        <div slot="footer" class="dialog-footer">
            <el-button @click="close">取 消</el-button>
            <el-button type="primary" @click="ok">确 定</el-button>
        </div>
    </el-dialog>
</template>
<script>
// import { addEdit } from "@/api/sys/role";
/**
 * 用户添加编辑
 */
export default {
    components: {},
    data() {
        return {
            show: false,
            loading: false,
            // 表单数据
            formData: {
                countryId: null,
                continent: null,
                twoCode: null,
                threeCode: null,
                otherCode: null,
                cnName: null,
                twName: null,
                enName: null,
                remark: null
            },
            formRules: {
                continent: [
                    { required: true, message: '请选择洲信息', trigger: 'change' }
                ],
                twoCode: [
                    { required: true, message: '请输入国家二字码', trigger: 'blur' },
                    { min: 2, max: 2, message: '长度为2个字符', trigger: 'blur' }
                ],
                threeCode: [
                    { required: false, message: '请输入国家三字码', trigger: 'blur' },
                    { min: 3, max: 3, message: '长度为3个字符', trigger: 'blur' }
                ],
                cnName: [
                    { required: true, message: '请输入中文名称', trigger: 'blur' }
                ],
                enName: [
                    { required: true, message: '请输入英文名称', trigger: 'blur' }
                ],
                twName: [
                    { required: true, message: '请输入简体中文名', trigger: 'blur' }
                ]
            }
        }
    },
    computed: {
        title() {
            return this.formData.countryId ? '编辑' : '添加'
        }
    },
    mounted() {},
    methods: {
        // 确定
        ok: function() {
            this.$refs.form.validate(valid => {
                if (valid) {
                    // this.loading = true;
                    // save
                    this.$message.success('验证成功')
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
                this.formData.countryId = null
                if (row) {
                    this.formData.countryId = row.countryId
                    this.formData.continent = row.continent
                    this.formData.twoCode = row.twoCode
                    this.formData.threeCode = row.threeCode
                    this.formData.otherCode = row.otherCode
                    this.formData.enName = row.enName
                    this.formData.cnName = row.cnName
                    this.formData.twName = row.twName
                    this.formData.remark = row.remark
                }
            })
        }
    }
}
</script>
