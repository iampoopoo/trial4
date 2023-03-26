/**
 * @file src/glShaderProgram.cpp
 * @author G'lek Tarssza
 * @copyright Copyright (c) 2023 G'lek Tarssza
 * @copyright All rights reserved.
 */

#include "glShaderProgram.hpp"

namespace glekcraft {
    GLShaderProgram::GLShaderProgram() {
        m_id = glCreateProgram();
        m_disposed = false;
        m_shaders = std::list<const GLShader*>();
    }

    GLShaderProgram::~GLShaderProgram() {
        Dispose();
    }

    GLuint GLShaderProgram::GetID() const {
        return m_id;
    }

    bool GLShaderProgram::IsDisposed() const {
        return m_disposed;
    }

    bool GLShaderProgram::IsLinked() const {
        if (IsDisposed()) {
            return false;
        }
        GLint status;
        glGetProgramiv(m_id, GL_LINK_STATUS, &status);
        return status == GL_TRUE;
    }

    bool GLShaderProgram::IsActive() const {
        if (IsDisposed()) {
            return false;
        }
        GLint status;
        glGetProgramiv(m_id, GL_CURRENT_PROGRAM, &status);
        return status == m_id;
    }

    bool GLShaderProgram::HasAttachedShader(const GLShader* shader) const {
        if (IsDisposed()) {
            return false;
        }
        return std::find(m_shaders.begin(), m_shaders.end(), shader) !=
               m_shaders.end();
    }

    std::string GLShaderProgram::GetInfoLog() const {
        if (IsDisposed()) {
            return std::string();
        }
        auto infoLog = std::string();
        auto infoLogLength = 0;
        glGetProgramiv(m_id, GL_INFO_LOG_LENGTH, &infoLogLength);
        if (infoLogLength > 0) {
            infoLog.resize(infoLogLength);
            glGetProgramInfoLog(m_id, infoLogLength, nullptr, &infoLog[0]);
        }
        return infoLog;
    }

    void GLShaderProgram::Dispose() {
        if (IsDisposed()) {
            return;
        }
        glDeleteProgram(m_id);
        m_shaders.clear();
        m_disposed = true;
    }

    void GLShaderProgram::AttachShader(const GLShader* shader) {
        if (IsDisposed()) {
            return;
        }
        if (HasAttachedShader(shader)) {
            return;
        }
        glAttachShader(m_id, shader->GetID());
        m_shaders.push_back(shader);
    }

    void GLShaderProgram::DetachShader(const GLShader* shader) {
        if (IsDisposed()) {
            return;
        }
        if (!HasAttachedShader(shader)) {
            return;
        }
        glDetachShader(m_id, shader->GetID());
        m_shaders.remove(shader);
    }

    void GLShaderProgram::Link() {
        if (IsDisposed()) {
            return;
        }
        glLinkProgram(m_id);
    }

    void GLShaderProgram::Activate() {
        if (IsDisposed()) {
            return;
        }
        if (IsActive()) {
            return;
        }
        glUseProgram(m_id);
    }

    void GLShaderProgram::Deactivate() {
        if (IsDisposed()) {
            return;
        }
        if (!IsActive()) {
            return;
        }
        glUseProgram(0);
    }
} // namespace glekcraft
