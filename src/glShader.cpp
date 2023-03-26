/**
 * @file src/glShader.cpp
 * @author G'lek Tarssza
 * @copyright Copyright (c) 2023 G'lek Tarssza
 * @copyright All rights reserved.
 */

#include "glShader.hpp"

namespace glekcraft {
    GLShader::GLShader(GLenum type) {
        m_id = glCreateShader(type);
        m_type = type;
        m_disposed = false;
    }

    GLShader::~GLShader() {
        Dispose();
    }

    GLuint GLShader::GetID() const {
        return m_id;
    }

    bool GLShader::IsDisposed() const {
        return m_disposed;
    }

    bool GLShader::IsCompiled() const {
        if (IsDisposed()) {
            return false;
        }
        GLint status;
        glGetShaderiv(m_id, GL_COMPILE_STATUS, &status);
        return status == GL_TRUE;
    }

    std::string GLShader::GetInfoLog() const {
        if (IsDisposed()) {
            return std::string();
        }
        auto infoLog = std::string();
        auto infoLogLength = 0;
        glGetShaderiv(m_id, GL_INFO_LOG_LENGTH, &infoLogLength);
        if (infoLogLength > 0) {
            infoLog.resize(infoLogLength);
            glGetShaderInfoLog(m_id, infoLogLength, nullptr, &infoLog[0]);
        }
        return infoLog;
    }

    void GLShader::Dispose() {
        if (IsDisposed()) {
            return;
        }
        glDeleteShader(m_id);
        m_disposed = true;
    }

    void GLShader::UploadSource(const char* source) {
        if (IsDisposed()) {
            return;
        }
        glShaderSource(m_id, 1, &source, nullptr);
    }

    void GLShader::UploadSource(const std::string& source) {
        UploadSource(source.c_str());
    }

    void GLShader::Compile() {
        if (IsDisposed()) {
            return;
        }
        glCompileShader(m_id);
    }
} // namespace glekcraft
