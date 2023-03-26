/**
 * @file src/glBuffer.cpp
 * @author G'lek Tarssza
 * @copyright Copyright (c) 2023 G'lek Tarssza
 * @copyright All rights reserved.
 */

#include "glBuffer.hpp"

namespace glekcraft {
    GLBuffer::GLBuffer(GLenum target) {
        m_disposed = false;
        m_target = target;
        glGenBuffers(1, &m_id);
    }

    GLBuffer::~GLBuffer() {
        Dispose();
    }

    GLuint GLBuffer::GetID() const {
        return m_id;
    }

    bool GLBuffer::IsDisposed() const {
        return m_disposed;
    }

    bool GLBuffer::IsBound() const {
        if (IsDisposed()) {
            return false;
        }
        GLenum targetPName;
        switch (m_target) {
        case GL_ARRAY_BUFFER:
            targetPName = GL_ARRAY_BUFFER_BINDING;
            break;
        case GL_ELEMENT_ARRAY_BUFFER:
            targetPName = GL_ELEMENT_ARRAY_BUFFER_BINDING;
            break;
        default:
            return false;
        }
        GLint boundBuffer;
        glGetIntegerv(targetPName, &boundBuffer);
        return boundBuffer == m_id;
    }

    void GLBuffer::Dispose() {
        if (IsDisposed()) {
            return;
        }
        glDeleteBuffers(1, &m_id);
        m_id = 0;
        m_disposed = true;
    }

    void GLBuffer::Bind() {
        if (IsDisposed()) {
            return;
        }
        if (IsBound()) {
            return;
        }
        glBindBuffer(m_target, m_id);
    }

    void GLBuffer::Unbind() {
        if (IsDisposed()) {
            return;
        }
        if (!IsBound()) {
            return;
        }
        glBindBuffer(m_target, 0);
    }
} // namespace glekcraft
