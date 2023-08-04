#pragma once
#include <vector>
#include <deque>
template<typename T>
class GC final
{
	
	struct node_type
	{
		T* first;
		int second;
	};
	std::deque<node_type> bank;
public:
	class Reference {
		friend class GC;
	protected:
		node_type* inst;
		constexpr Reference(node_type* in) : inst(in) {
			inst->second++;
		}
	public:
		Reference() = delete;
		constexpr T& operator*() {
			return *inst->first;
		}
		constexpr T* operator->() {
			return inst->first;
		}
		constexpr ~Reference() {
			inst->second--;
		}
		constexpr Reference(const Reference& in) {
			inst = in.inst;
			inst->second++;
		}
		constexpr Reference& operator=(const Reference& in) {
			inst->second--;
			inst = in.inst;
			inst->second++;
			return *this;		
		}
	};	
	[[nodiscard]] Reference New() {
		node_type newRef{new T[1],0};

		for (node_type& i : bank)
		{		
			if (i.first == nullptr) {
				i = newRef;				
				return Reference(&i);
			}
		}
		bank.push_back(newRef);
		return Reference(&bank.back());
	}
	[[nodiscard]]	
	void Collect() {
		for (node_type& i : bank)
		{
			if (i.second <= 0) {
				delete[] i.first;
				i.first = nullptr;
			}
		}
	}
	~GC()
	{
		for (auto& i : bank)
		{
			delete[] i.first;
		}
	}
	 GC(const GC&) = delete;
	 GC() = default;
};


