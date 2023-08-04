#pragma once
#include <vector>
#include <map>
#include <string>
#include <deque>
std::string ToString(long double in);
 template<class T>
class OpenGC {
	static std::deque<std::pair<T*, size_t>> bank;
	// why didn't i just use a stack? because stack does not have a size limit
	static unsigned int FreeList[100];
	static int FreeIndex;
	static bool OverFlown;
	static bool PushFree(unsigned int token) {
		if (FreeIndex < 99) {
			FreeList[++FreeIndex] = token;
			return true;			
		}
		else {
			OverFlown = true;
			return false;
		}
	}
	static unsigned int PopFree() {
		if (FreeIndex > -1) {
			return FreeList[FreeIndex--];
		}
		else return -1;
	}
	static void SearchForFree() {
		FreeIndex = -1;
		OverFlown = false;
		for (unsigned int i = 0; i < bank.size(); i++)
		{
			if (bank[i].first != 0) continue;
			if (!PushFree(i)) return;
		}
	}
public:
	static unsigned int CreateReference(T* in) {
		unsigned int index = PopFree();
		if (index == -1 && OverFlown) {
			SearchForFree();
			index = PopFree();
		}
		if (index == -1) {
			bank.push_back({ in,1 });
			return (unsigned int) bank.size() - 1;
		}
		else {
			bank[index] = { in,1 };
			return index;
		}
	}

	static void CopyReference(unsigned int token) {
		bank[token].second++;
	}
	static const std::pair<void*, size_t>& Dereference(unsigned int token) {
		return bank[token];
	}
	static void DestroyReference(unsigned int token) {
		auto& it = bank[token];
		if (--it.second == 0) {

			delete it.first;
			it.first = 0;
			it.second = 0;
		}
	}
};
template<typename T>
int OpenGC<T>::FreeIndex = -1;
template<typename T>
bool OpenGC<T>::OverFlown = false;
template<typename T>
unsigned int OpenGC<T>::FreeList[100]{};
template<typename T>
std::deque<std::pair<T*, size_t>> OpenGC<T>::bank{};